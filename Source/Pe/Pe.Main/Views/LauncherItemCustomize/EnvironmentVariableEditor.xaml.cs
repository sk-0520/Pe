using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace ContentTypeTextNet.Pe.Main.Views.LauncherItemCustomize
{
    /// <summary>
    /// EnvironmentVariableEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class EnvironmentVariableEditor: UserControl
    {
        public EnvironmentVariableEditor()
        {
            InitializeComponent();
            var dic = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            var envVariables = new Dictionary<string, string>(dic.Count);
            foreach(var kv in dic.Cast<DictionaryEntry>()) {
                envVariables[(string)kv.Key] = (string)kv.Value!;
            }
            EnvironmentVariables = envVariables;
        }

        #region property

        private IReadOnlyDictionary<string, string> EnvironmentVariables { get; }
        private CompletionWindow? EnvRemoveEditorCompletionWindow { get; set; }

        #endregion

        #region MergeTextDocumentProperty

        public static readonly DependencyProperty MergeTextDocumentProperty = DependencyProperty.Register(
            nameof(MergeTextDocument),
            typeof(TextDocument),
            typeof(EnvironmentVariableEditor),
            new PropertyMetadata(
                new TextDocument(),
                OnMergeTextDocumentChanged
            )
        );

        public TextDocument MergeTextDocument
        {
            get { return (TextDocument)GetValue(MergeTextDocumentProperty); }
            set { SetValue(MergeTextDocumentProperty, value); }
        }

        private static void OnMergeTextDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is EnvironmentVariableEditor control) {
                control.MergeTextDocument = (TextDocument)e.NewValue;
            }
        }

        #endregion

        #region RemoveTextDocumentProperty

        public static readonly DependencyProperty RemoveTextDocumentProperty = DependencyProperty.Register(
            nameof(RemoveTextDocument),
            typeof(TextDocument),
            typeof(EnvironmentVariableEditor),
            new PropertyMetadata(
                new TextDocument(),
                OnRemoveTextDocumentChanged
            )
        );

        public TextDocument RemoveTextDocument
        {
            get { return (TextDocument)GetValue(RemoveTextDocumentProperty); }
            set { SetValue(RemoveTextDocumentProperty, value); }
        }

        private static void OnRemoveTextDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is EnvironmentVariableEditor control) {
                control.RemoveTextDocument = (TextDocument)e.NewValue;
            }
        }

        #endregion

        #region MergeErrorItemsSourceProperty

        public static readonly DependencyProperty MergeErrorItemsSourceProperty = DependencyProperty.Register(
            nameof(MergeErrorItemsSource),
            typeof(IEnumerable),
            typeof(EnvironmentVariableEditor),
            new PropertyMetadata(
                new PropertyChangedCallback(OnMergeErrorItemsSourcePropertyChanged)
            )
        );

        public IEnumerable MergeErrorItemsSource
        {
            get { return (IEnumerable)GetValue(MergeErrorItemsSourceProperty); }
            set { SetValue(MergeErrorItemsSourceProperty, value); }
        }

        private static void OnMergeErrorItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is EnvironmentVariableEditor control) {
                control.OnMergeErrorItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
            }
        }

        private void OnMergeErrorItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if(null != oldValueINotifyCollectionChanged) {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(MergeErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }

            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if(null != newValueINotifyCollectionChanged) {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(MergeErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }
        }

        void MergeErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        { }

        #endregion

        #region RemoveErrorItemsSourceProperty

        public static readonly DependencyProperty RemoveErrorItemsSourceProperty = DependencyProperty.Register(
            nameof(RemoveErrorItemsSource),
            typeof(IEnumerable),
            typeof(EnvironmentVariableEditor),
            new PropertyMetadata(
                new PropertyChangedCallback(OnRemoveErrorItemsSourcePropertyChanged)
            )
        );

        public IEnumerable RemoveErrorItemsSource
        {
            get { return (IEnumerable)GetValue(RemoveErrorItemsSourceProperty); }
            set { SetValue(RemoveErrorItemsSourceProperty, value); }
        }


        private static void OnRemoveErrorItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is EnvironmentVariableEditor control) {
                control.OnRemoveErrorItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
            }
        }

        private void OnRemoveErrorItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if(null != oldValueINotifyCollectionChanged) {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(RemoveErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }

            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if(null != newValueINotifyCollectionChanged) {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(RemoveErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }
        }

        void RemoveErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        { }

        #endregion

        private void EnvMergeEditor_Loaded(object sender, RoutedEventArgs e)
        {
            using(var stream = ResourceUtility.OpenSyntaxStream(Properties.Resources.File_Highlighting_EnvironmentVariable_Merge)) {
                AvalonEditHelper.SetSyntaxHighlightingDefault((ICSharpCode.AvalonEdit.TextEditor)sender, stream);
            }
        }

        private void EnvRemoveEditor_Loaded(object sender, RoutedEventArgs e)
        {
            using(var stream = ResourceUtility.OpenSyntaxStream(Properties.Resources.File_Highlighting_EnvironmentVariable_Remove)) {
                AvalonEditHelper.SetSyntaxHighlightingDefault((ICSharpCode.AvalonEdit.TextEditor)sender, stream);
            }
        }

        private void envRemoveEditor_Initialized(object sender, System.EventArgs e)
        {
            this.envRemoveEditor.TextArea.TextEntering += envRemoveEditor_TextArea_TextEntering;
        }

        private void envRemoveEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) {
                e.Handled = true;
                EnvRemoveEditorCompletionWindow = new CompletionWindow(this.envRemoveEditor.TextArea);
                var data = EnvRemoveEditorCompletionWindow.CompletionList.CompletionData;
                foreach(var kv in EnvironmentVariables.OrderBy(a => a.Key)) {
                    data.Add(new EnvRemoveCompletionData(kv.Key, kv.Value));
                }
                EnvRemoveEditorCompletionWindow.Show();
            }
        }

        private void envRemoveEditor_TextArea_TextEntering(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if(e.Text.Length > 0 && EnvRemoveEditorCompletionWindow != null) {
                if(!char.IsLetterOrDigit(e.Text[0])) {
                    EnvRemoveEditorCompletionWindow.CompletionList.RequestInsertion(e);
                }
            }
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
            this.envRemoveEditor.TextArea.TextEntering -= envRemoveEditor_TextArea_TextEntering;
            if(EnvRemoveEditorCompletionWindow is not null) {
                EnvRemoveEditorCompletionWindow.Close();
                EnvRemoveEditorCompletionWindow = null;
            }
        }
    }
}
