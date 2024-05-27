using MyStoreManage.Models;
using MyStoreManage.session_login;
using System.Windows;

namespace MyStoreManage
{
    /// <summary>
    /// Interaction logic for WindowStaffManage.xaml
    /// </summary>
    public partial class WindowStaffManage : Window
    {
        private readonly MyStoreContext _storeContext;
        public WindowStaffManage(MyStoreContext storeContext)
        {
            InitializeComponent();
            _storeContext = storeContext;
            HandleStaffNameNavigate();
            HandleButttonRole();
        }
        public void HandleButttonRole()
        {
            var role = SessionService.Instance.GetRoleInSession();
            if (role == 0)
            {
                btnOpenOrdersManage.Visibility = Visibility.Hidden;
            }
            else if (role == 1)
            {
                btnOpenProductsManage.Visibility = Visibility.Hidden;
                btnOpenCategoriesManage.Visibility = Visibility.Hidden;
                btnOpenStaffManage.Visibility = Visibility.Hidden;
            }
        }
        public void HandleStaffNameNavigate()
        {
            txblStaffNameNavigate.Text = SessionService.Instance.GetNameInSession();
        }


        private void Staff_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStaff();
        }

        private void LoadStaff()
        {
            lvStaff.ItemsSource = _storeContext.Staffs.ToList();

        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_storeContext.Staffs.FirstOrDefault(x => x.Name == txtStaffName.Text) == null)
                {
                    var staffNew = new Staff
                    {
                        Name = txtStaffName.Text,
                        Password = txtPassword.Text,
                        Role = int.Parse(txtRole.Text)
                    };
                    _storeContext.Staffs.Add(staffNew);
                    _storeContext.SaveChanges();
                    LoadStaff();
                }
                else
                {
                    MessageBox.Show("Exsit Staff");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Insert Staff");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var existingStaff = _storeContext.Staffs.FirstOrDefault(x => x.StaffId == int.Parse(txtStaffID.Text));
                if (existingStaff != null)
                {
                    existingStaff.Name = txtStaffName.Text;
                    existingStaff.Password = txtPassword.Text;
                    existingStaff.Role = int.Parse(txtRole.Text);
                    _storeContext.SaveChanges();
                    LoadStaff();
                }
                else
                {
                    MessageBox.Show("Exsit Staff");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Staff");
            }
        }



        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Hiển thị hộp thoại xác nhận
                MessageBoxResult result = MessageBox.Show("Do you want to delete?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                // Nếu người dùng chọn Yes
                if (result == MessageBoxResult.Yes)
                {
                    var staffToDelete = _storeContext.Staffs.FirstOrDefault(x => x.StaffId == int.Parse(txtStaffID.Text));
                    if (staffToDelete != null)
                    {
                        _storeContext.Staffs.Remove(staffToDelete);
                        _storeContext.SaveChanges();
                        LoadStaff();
                    }
                    else
                    {
                        MessageBox.Show("Staff does not exist");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Staff");
            }
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim(); // Lấy từ khóa tìm kiếm từ TextBox

            var foundStaff = _storeContext.Staffs
                .Where(x => x.Name.Contains(searchText))
                .ToList();
            lvStaff.ItemsSource = foundStaff;
        }
        //ToolBar
        private void btnOpenOrdersManage_Click(object sender, RoutedEventArgs e)
        {
            var windowOrderManage = new WindowOrderManage(_storeContext);
            this.Close();
            windowOrderManage.Show();
            e.Handled = true;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionService.Instance.ClearSession();
            var mainWindow = new MainWindow(_storeContext);
            this.Close();
            mainWindow.Show();
            e.Handled = true;
        }

        private void btnOpenProductsManage_Click(object sender, RoutedEventArgs e)
        {
            var windowProductManage = new WindowProductManage(_storeContext);
            this.Close();
            windowProductManage.Show();
            e.Handled = true;
        }
        private void btnOpenCategoriesManage_Click(object sender, RoutedEventArgs e)
        {
            var windowCategoryManage = new WindowCategoryManage(_storeContext);
            this.Close();
            windowCategoryManage.Show();
            e.Handled = true;
        }
        private void btnOpenMyAccount_Click(object sender, RoutedEventArgs e)
        {
            var WindowMyProfile = new WindowMyProfile(_storeContext);
            this.Close();
            WindowMyProfile.Show();
            e.Handled = true;
        }

        private void btnOpenOrdersReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOpenReport_Click(object sender, RoutedEventArgs e)
        {
            var windowReport = new WindowReport(_storeContext);
            this.Close();
            windowReport.Show();
            e.Handled = true;
        }
    }
}
