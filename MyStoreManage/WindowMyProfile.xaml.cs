using MyStoreManage.Models;
using MyStoreManage.session_login;
using System.Windows;

namespace MyStoreManage
{
    /// <summary>
    /// Interaction logic for WindowMyProfile.xaml
    /// </summary>
    public partial class WindowMyProfile : Window
    {
        private readonly MyStoreContext _context;
        public WindowMyProfile(MyStoreContext context)
        {
            InitializeComponent();
            _context = context;
            HandleStaffInfoBeforeLoaded();
            HandleStaffNameNavigate();
        }
        public void HandleStaffNameNavigate()
        {
            txblStaffNameNavigate.Text = SessionService.Instance.GetNameInSession();
        }
        public void HandleStaffInfoBeforeLoaded()
        {
            var staffInfo = _context.Staffs.FirstOrDefault(x => x.StaffId == SessionService.Instance.GetStaffIdInSession());
            if (staffInfo != null)
            {
                txtStaffID.Text = staffInfo.StaffId.ToString();
                txtStaffName.Text = staffInfo.Name;
                txtRole.Text = staffInfo.Role.ToString();
            }
        }

        //ToolBar
        private void btnOpenOrdersManage_Click(object sender, RoutedEventArgs e)
        {
            var windowOrderManage = new WindowOrderManage(_context);
            this.Close();
            windowOrderManage.Show();
            e.Handled = true;
        }

        private void btnOpenStaffManage_Click(object sender, RoutedEventArgs e)
        {
            var windowStaffManage = new WindowStaffManage(_context);
            this.Close();
            windowStaffManage.Show();
            e.Handled = true;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(_context);
            this.Close();
            mainWindow.Show();
            e.Handled = true;
        }

        private void btnOpenProductsManage_Click(object sender, RoutedEventArgs e)
        {
            var windowProductManage = new WindowProductManage(_context);
            this.Close();
            windowProductManage.Show();
            e.Handled = true;
        }
        private void btnOpenCategoriesManage_Click(object sender, RoutedEventArgs e)
        {
            var windowCategoryManage = new WindowCategoryManage(_context);
            this.Close();
            windowCategoryManage.Show();
            e.Handled = true;
        }

        private void btnOpenReport_Click(object sender, RoutedEventArgs e)
        {
            var windowReport = new WindowReport(_context);
            this.Close();
            windowReport.Show();
            e.Handled = true;
        }

        private void btnEditName_Click(object sender, RoutedEventArgs e)
        {
            txtStaffName.IsReadOnly = false;
            stpButtonChangeName.Visibility = Visibility.Visible;
            stpButtonHandle.Visibility = Visibility.Hidden;
        }
        private void btnCancelName_Click(object sender, RoutedEventArgs e)
        {
            txtStaffName.IsReadOnly = true;
            stpButtonChangeName.Visibility = Visibility.Hidden;
            stpButtonHandle.Visibility = Visibility.Visible;
            txtStaffName.Text = SessionService.Instance.GetNameInSession();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            stpButtonHandle.Visibility = Visibility.Hidden;
            stpPasswordTextBox.Visibility = Visibility.Visible;
            stpButtonPassWord.Visibility = Visibility.Visible;
            rtgForm.Height = 400;

        }
        private void btnCancelPassWord_Click(object sender, RoutedEventArgs e)
        {
            stpButtonHandle.Visibility = Visibility.Visible;
            stpPasswordTextBox.Visibility = Visibility.Hidden;
            stpButtonPassWord.Visibility = Visibility.Hidden;
            rtgForm.Height = 230;
        }
        private void btnUpdatePassWord_Click(object sender, RoutedEventArgs e)
        {
            var oldPassWord = txtOldPassword.Password;
            var password = txtPassword.Password;
            var password2 = txtPassword_2.Password;
            var staffInfor = _context.Staffs.FirstOrDefault(x => x.StaffId == SessionService.Instance.GetStaffIdInSession());
            if (staffInfor != null && staffInfor.Password == oldPassWord)
            {
                if (password == password2)
                {
                    staffInfor.Password = password;
                    _context.Update(staffInfor);
                    _context.SaveChanges();
                    MessageBox.Show("Edit password success. Please login again!", "Update Password", MessageBoxButton.OK, MessageBoxImage.Information);
                    SessionService.Instance.ClearSession();
                    var mainWindow = new MainWindow(_context);
                    this.Close();
                    mainWindow.Show();
                    e.Handled = true;
                }
                else
                {
                    MessageBox.Show("New password don't match with re-password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid old password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdateName_Click(object sender, RoutedEventArgs e)
        {
            var staffName = txtStaffName.Text.Trim();
            int staffId = Int32.Parse(txtStaffID.Text);
            var oldStaffInfo = _context.Staffs.FirstOrDefault(x => x.StaffId == staffId);

            if (oldStaffInfo.Name == staffName)
            {
                //nguoi dung chua thay doi ten dang nhap thi khong lam gi
            }
            else if (_context.Staffs.Any(x => x.Name == staffName && x.StaffId != staffId))
            {
                MessageBox.Show("Staff name already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(staffName))
            {
                MessageBox.Show("Staff name must not be blank!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                oldStaffInfo.Name = staffName;
                _context.Update(oldStaffInfo);
                _context.SaveChanges();
                MessageBox.Show("Edit name staff success. Please login again!", "Update Staff Name", MessageBoxButton.OK, MessageBoxImage.Information);
                SessionService.Instance.ClearSession();
                var mainWindow = new MainWindow(_context);
                this.Close();
                mainWindow.Show();
                e.Handled = true;
            }
        }

        private void btnOpenMyAccount_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
