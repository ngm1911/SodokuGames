using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace WpfSodokuApp.ViewModel
{
    public partial class BlockViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Cell0))]
        [NotifyPropertyChangedFor(nameof(Cell1))]
        [NotifyPropertyChangedFor(nameof(Cell2))]
        [NotifyPropertyChangedFor(nameof(Cell3))]
        [NotifyPropertyChangedFor(nameof(Cell4))]
        [NotifyPropertyChangedFor(nameof(Cell5))]
        [NotifyPropertyChangedFor(nameof(Cell6))]
        [NotifyPropertyChangedFor(nameof(Cell7))]
        [NotifyPropertyChangedFor(nameof(Cell8))]
        public ObservableCollection<CellViewModel> cells = new ObservableCollection<CellViewModel>();

        public CellViewModel Cell0 => Cells[0];
        public CellViewModel Cell1 => Cells[1];
        public CellViewModel Cell2 => Cells[2];
        public CellViewModel Cell3 => Cells[3];
        public CellViewModel Cell4 => Cells[4];
        public CellViewModel Cell5 => Cells[5];
        public CellViewModel Cell6 => Cells[6];
        public CellViewModel Cell7 => Cells[7];
        public CellViewModel Cell8 => Cells[8];
    }
}
