using MobileRequestsService.Views;

namespace MobileRequestsService
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(
               nameof(CreateDocumentOrderView), typeof(CreateDocumentOrderView));
        }
    }
}
