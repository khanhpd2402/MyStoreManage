using MyStoreManage.Models;
using MyStoreManage.session_login;
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
            // Trong bất kỳ lớp nào muốn sử dụng SessionService
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var staffName = txtStaffName.Text;
            var password = txtPass.Password;
            if(staffName.Trim() != null)
            {
                var aStaff = _storeContext.Staffs.FirstOrDefault(x => x.Name.Equals(staffName));
                if(aStaff != null && aStaff.Password.Equals(password)) {
                    //handle session
                    SessionService.Instance.SetSession(aStaff.StaffId, aStaff.Role, aStaff.Name);
                    //admin dang nhap thi den product
                    if(aStaff.Role == 1)
                    {
                        var windowProductManage = new WindowProductManage(_storeContext);
                        windowProductManage.Show();
                    }//staff dang nhap thi den order cua staff do
                    else if(aStaff.Role == 2)
                    {
                        var windowOrderManage = new WindowOrderManage(_storeContext);
                        windowOrderManage.Show();
                    }
                    this.Close();
                    e.Handled = true;
                }
                else
                {
                    MessageBox.Show("Invalid name or password", "Error!", MessageBoxButton.OK ,MessageBoxImage.Error);
                }
            }
        }
    }
}