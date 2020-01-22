using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InterGraph_Labo8
{
    public enum Acreditation { Low, Medium, High }
    public class Profil
    {
        private string _password;

        public Profil() { }
        public Profil(string profileName, Acreditation acreditationLevel, string password, string imageSource)
        {
            ProfilName = profileName;
            AcreditationLevel = acreditationLevel;
            Password = password;
            ImageSource = imageSource;
        }

        #region Propreties
        public string ImageSource { get; set; }
        public string ProfilName { get; set; }
        public Acreditation AcreditationLevel { get; set; }
        public string Password {
            set
            {
                _password = value;
            }
            get { return _password; }
        }
        #endregion

        public bool ComparisonPassword(string input)
        {
            return input == _password;
        }
    }
}
