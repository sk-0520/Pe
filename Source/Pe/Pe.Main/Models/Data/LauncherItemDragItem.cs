using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public record class LauncherItemDragItem(
        UIElement View,
        LauncherDetailViewModelBase ViewModel
    );
}
