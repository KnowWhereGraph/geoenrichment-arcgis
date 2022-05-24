using System.Windows.Forms;
using Button = ArcGIS.Desktop.Framework.Contracts.Button;

namespace KWG_Geoenrichment
{
    internal class BeginGeoenrichment : Button
    {
        protected override void OnClick()
        {
            Form geoenrichmentForm = new GeoenrichmentForm();
            geoenrichmentForm.ShowDialog();
        }
    }
}
