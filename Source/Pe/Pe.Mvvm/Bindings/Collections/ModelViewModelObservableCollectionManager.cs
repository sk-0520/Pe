using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Mvvm.Bindings.Collections
{
    /// <summary>
    /// <typeparamref name="TModel"/> と <typeparamref name="TViewModel"/> の一元的管理。
    /// <para>対になっている部分は内部で対応するがその前後処理までは面倒見ない。</para>
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class ModelViewModelObservableCollectionManager<TModel, TViewModel>: ObservableCollectionManagerBase<TModel>
        where TViewModel : INotifyPropertyChanged
    {
        #region variable

        private ReadOnlyObservableCollection<TViewModel>? _readOnlyViewModels;

        #endregion

        public ModelViewModelObservableCollectionManager(ReadOnlyObservableCollection<TModel> collection, ModelViewModelObservableCollectionOptions<TModel, TViewModel> options)
            : base(collection)
        {
            if(options.ToViewModel == null) {
                throw new ArgumentNullException(nameof(options) + "." + nameof(options.ToViewModel));
            }

            Options = options;
            EditableViewModels = new ObservableCollection<TViewModel>(Collection.Select(m => ToViewModelCore(m)));
        }

        public ModelViewModelObservableCollectionManager(ObservableCollection<TModel> collection, ModelViewModelObservableCollectionOptions<TModel, TViewModel> options)
            : base(collection)
        {
            if(options.ToViewModel == null) {
                throw new ArgumentNullException(nameof(options) + "." + nameof(options.ToViewModel));
            }

            Options = options;
            EditableViewModels = new ObservableCollection<TViewModel>(Collection.Select(m => ToViewModelCore(m)));
        }

        #region property

        private ModelViewModelObservableCollectionOptions<TModel, TViewModel> Options { get; set; }

        /// <summary>
        /// 内部使用する<typeparamref name="TViewModel"/>のコレクション。
        /// </summary>
        protected ObservableCollection<TViewModel> EditableViewModels { get; private set; }
        /// <summary>
        /// 外部使用する<typeparamref name="TViewModel"/>のコレクション。
        /// </summary>
        public ReadOnlyObservableCollection<TViewModel> ViewModels => this._readOnlyViewModels ??= new ReadOnlyObservableCollection<TViewModel>(EditableViewModels);

        #endregion

        #region function

        /// <summary>
        /// <typeparamref name="TModel"/>を<typeparamref name="TViewModel"/>に変換する。
        /// </summary>
        /// <param name="model"></param>
        /// <returns>初期化前の場合は null、初期化後は生成後の<typeparamref name="TViewModel"/>。</returns>
        protected TViewModel ToViewModelCore(TModel model)
        {
            return Options.ToViewModel(model);
        }

        protected void AddItemsKindCore(ModelViewModelObservableCollectionOptions<TModel, TViewModel>.AddItemParameter parameter)
        {
            Options.AddItems?.Invoke(parameter);
        }

        protected void InsertItemsKindCore(ModelViewModelObservableCollectionOptions<TModel, TViewModel>.InsertItemParameter parameter)
        {
            Options.InsertItems?.Invoke(parameter);
        }

        protected void RemoveItemsKindCore(ModelViewModelObservableCollectionOptions<TModel, TViewModel>.RemoveItemParameter parameter)
        {
            Options.RemoveItems?.Invoke(parameter);
        }

        protected void ReplaceItemsKindCore(ModelViewModelObservableCollectionOptions<TModel, TViewModel>.ReplaceItemParameter parameter)
        {
            Options.ReplaceItems?.Invoke(parameter);
        }

        protected void MoveItemsKindCore(ModelViewModelObservableCollectionOptions<TModel, TViewModel>.MoveItemParameter parameter)
        {
            Options.MoveItems?.Invoke(parameter);
        }

        protected void ResetItemsKindCore(ModelViewModelObservableCollectionOptions<TModel, TViewModel>.ResetItemParameter parameter)
        {
            Options.ResetItems?.Invoke(parameter);
        }

        public ICollectionView GetDefaultView()
        {
            return CollectionViewSource.GetDefaultView(EditableViewModels);
        }

        public ICollectionView CreateView()
        {
            return new ListCollectionView(EditableViewModels);
        }

        public int IndexOf(TViewModel viewModel) => EditableViewModels.IndexOf(viewModel);

        public bool TryGetModel(TViewModel viewModel, [MaybeNullWhen(false)] out TModel result)
        {
            var index = IndexOf(viewModel);

            if(index == -1) {
                result = default;
                return false;
            }

            result = Collection[index];
            return true;
        }

        public bool TryGetViewModel(TModel model, [MaybeNullWhen(false)] out TViewModel result)
        {
            var index = IndexOf(model);

            if(index == -1) {
                result = default;
                return false;
            }

            result = EditableViewModels[index];
            return true;
        }

        /// <summary>
        /// ViewModel を解放する。
        /// </summary>
        /// <remarks><see cref="IDisposable"/>を実装している場合にのみ実行される。</remarks>
        /// <param name="viewModel"></param>
        /// <returns><see cref="IDisposable.Dispose"/>が実行されたか。</returns>
        private bool DisposeViewModelIfDisposable(TViewModel viewModel)
        {
            if(viewModel is IDisposable disposable) {
                disposable.Dispose();
                return true;
            }

            return false;
        }

        /// <summary>
        /// <see cref="ModelViewModelObservableCollectionOptions{TModel, TViewModel}.AutoDisposeViewModel"/> が真の場合に指定の ViewModel 一覧を解放する。
        /// </summary>
        /// <remarks>古いデータの解放を想定している。</remarks>
        /// <param name="viewModels"></param>
        protected void DisposeViewModelsIfAutoDispose(IEnumerable<TViewModel> viewModels)
        {
            if(Options.AutoDisposeViewModel) {
                foreach(var viewModel in viewModels) {
                    DisposeViewModelIfDisposable(viewModel);
                }
            }
        }

        #endregion

        #region ObservableCollectionManagerBase

        protected override void AddItemsCore(IReadOnlyList<TModel> newItems)
        {
            var newViewModels = newItems
                .Select(m => ToViewModelCore(m))
                .ToArray()
            ;

            var parameter = new ModelViewModelObservableCollectionOptions<TModel, TViewModel>.AddItemParameter() {
                Sender = this,
                Apply = ModelViewModelObservableCollectionViewModelApply.Before,
                NewModels = newItems,
                NewViewModels = newViewModels,
            };

            AddItemsKindCore(parameter);

            foreach(var vm in newViewModels) {
                EditableViewModels.Add(vm);
            }

            parameter.Apply = ModelViewModelObservableCollectionViewModelApply.After;
            AddItemsKindCore(parameter);
        }

        protected override void InsertItemsCore(int insertIndex, IReadOnlyList<TModel> newItems)
        {
            var newViewModels = newItems
                .Select(m => ToViewModelCore(m))
                .Select((v, i) => (index: i + insertIndex, value: v))
                .ToArray()
            ;

            var parameter = new ModelViewModelObservableCollectionOptions<TModel, TViewModel>.InsertItemParameter() {
                Sender = this,
                Apply = ModelViewModelObservableCollectionViewModelApply.Before,
                InsertIndex = insertIndex,
                NewModels = newItems,
                NewViewModels = newViewModels.Select(a => a.value).ToArray(),
            };

            InsertItemsKindCore(parameter);

            foreach(var (index, value) in newViewModels) {
                EditableViewModels.Insert(index, value);
            }

            parameter.Apply = ModelViewModelObservableCollectionViewModelApply.After;
            InsertItemsKindCore(parameter);
        }


        protected override void RemoveItemsCore(int oldStartingIndex, IReadOnlyList<TModel> oldItems)
        {
            var oldViewModels = EditableViewModels
                .Skip(oldStartingIndex)
                .Take(oldItems.Count)
                .ToArray()
            ;

            var parameter = new ModelViewModelObservableCollectionOptions<TModel, TViewModel>.RemoveItemParameter() {
                Sender = this,
                Apply = ModelViewModelObservableCollectionViewModelApply.Before,
                OldStartingIndex = oldStartingIndex,
                OldModels = oldItems,
                OldViewModels = oldViewModels,
            };

            RemoveItemsKindCore(parameter);

            foreach(var _ in Enumerable.Range(0, oldViewModels.Length)) {
                EditableViewModels.RemoveAt(oldStartingIndex);
            }
            DisposeViewModelsIfAutoDispose(oldViewModels);

            parameter.Apply = ModelViewModelObservableCollectionViewModelApply.After;
            RemoveItemsKindCore(parameter);
        }

        protected override void ReplaceItemsCore(int startIndex, IReadOnlyList<TModel> newItems, IReadOnlyList<TModel> oldItems)
        {
            //TODO: インデックスが必要
            var newViewModels = newItems
                .Select(m => ToViewModelCore(m))
                .ToArray()
            ;

            var oldViewModels = EditableViewModels
                .Skip(startIndex)
                .Take(oldItems.Count)
                .ToArray()
            ;

            var parameter = new ModelViewModelObservableCollectionOptions<TModel, TViewModel>.ReplaceItemParameter() {
                Sender = this,
                Apply = ModelViewModelObservableCollectionViewModelApply.Before,
                NewModels = newItems,
                NewViewModels = newViewModels,
                OldModels = oldItems,
                OldViewModels = oldViewModels,
                StartIndex = startIndex,
            };

            ReplaceItemsKindCore(parameter);

            for(var i = 0; i < newViewModels.Length; i++) {
                EditableViewModels[i + startIndex] = newViewModels[i];
            }
            //if (Options.AutoDisposeViewModel)
            //{
            //    foreach (var oldViewModel in oldViewModels)
            //    {
            //        oldViewModel.Dispose();
            //    }
            //}

            parameter.Apply = ModelViewModelObservableCollectionViewModelApply.After;
            ReplaceItemsKindCore(parameter);
        }

        protected override void MoveItemsCore(int newStartingIndex, int oldStartingIndex)
        {
            var parameter = new ModelViewModelObservableCollectionOptions<TModel, TViewModel>.MoveItemParameter() {
                Sender = this,
                Apply = ModelViewModelObservableCollectionViewModelApply.Before,
                NewStartingIndex = newStartingIndex,
                OldStartingIndex = oldStartingIndex,
            };

            MoveItemsKindCore(parameter);

            EditableViewModels.Move(oldStartingIndex, newStartingIndex);

            parameter.Apply = ModelViewModelObservableCollectionViewModelApply.After;
            MoveItemsKindCore(parameter);
        }

        protected override void ResetItemsCore()
        {
            var oldViewModels = EditableViewModels;

            var parameter = new ModelViewModelObservableCollectionOptions<TModel, TViewModel>.ResetItemParameter() {
                Sender = this,
                Apply = ModelViewModelObservableCollectionViewModelApply.Before,
                OldViewModels = oldViewModels,
            };

            ResetItemsKindCore(parameter);

            EditableViewModels.Clear();
            DisposeViewModelsIfAutoDispose(oldViewModels);

            parameter.Apply = ModelViewModelObservableCollectionViewModelApply.After;
            ResetItemsKindCore(parameter);
        }

        protected override void CollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            //Application.Current.Dispatcher.Invoke(new Action(() => base.CollectionChanged(e)));
            if(Options.SynchronizationContext != null && Options.SynchronizationContext != SynchronizationContext.Current) {
                Options.SynchronizationContext.Send(_ => base.CollectionChanged(e), null);
            } else {
                base.CollectionChanged(e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    var oldItems = EditableViewModels.ToArray();
                    EditableViewModels.Clear();
                    DisposeViewModelsIfAutoDispose(oldItems);
                }
                Options = null!;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
