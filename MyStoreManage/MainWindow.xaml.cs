using MyStoreManage.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyStoreManage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MyStoreContext _storeContext;
        public MainWindow(MyStoreContext storeContext)
        {
            InitializeComponent();
            _storeContext = storeContext;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var staffName = txtStaffName.Text;
            var password = txtPass.Password;
            if(staffName.Trim() != null)
            {
                var aStaff = _storeContext.Staffs.FirstOrDefault(x => x.Name.Equals(staffName));
                if(aStaff != null && aStaff.Password.Equals(password)) {
                    var windowProductManage = new WindowProductManage(_storeContext);
                    this.Close();
                    windowProductManage.Show();
                    e.Handled = true;
                }
                else
                {
                    MessageBox.Show("Invalid staff name or password", "Error!", MessageBoxButton.OK ,MessageBoxImage.Error);
                }
            }
        }
    }
}