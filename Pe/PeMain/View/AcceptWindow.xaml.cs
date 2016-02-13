﻿/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
using ContentTypeTextNet.Pe.PeMain.ViewModel;

namespace ContentTypeTextNet.Pe.PeMain.View
{
    /// <summary>
    /// AcceptWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AcceptWindow: ViewModelCommonDataWindow<AcceptViewModel>
    {
        public AcceptWindow()
        {
            InitializeComponent();
        }

        #region CommonDataWindow

        protected override void CreateViewModel()
        {
            ViewModel = new AcceptViewModel(CommonData.MainSetting.RunningInformation, this);
        }

        protected override void ApplyViewModel()
        {
            DataContext = ViewModel;

            ViewModel.SetAcceptDocument(webDocument, CommonData.Language, CommonData.VariableConstants);

            base.ApplyViewModel();
        }

        #endregion
    }
}
