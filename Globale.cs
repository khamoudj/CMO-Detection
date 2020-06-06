using Microsoft.Data.ConnectionUI;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Management;
using System.Collections;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.ComponentModel;

namespace Bearings
{
    public class Globale
    {
        #region "Déclarations globales"        
        public static bool op1 = false;
        public static string NumCheque = "";
        public static int OpenEdition = 0;
        public static int U_id = -1;
        public static string U_username = "";
        public static string U_nom = "";
        public static string U_password = "";
        public static string U_fonction = "";
        public static string U_niveauacces = "";
        public static int U_actif = -1;

        public static string Chambre = "";
        public static string NbJours = "";
        public static DateTime dat = DateTime.Today;
        public static DateTime dtReservation = DateTime.Today;
        public static bool showerrors = true;
        public static string ConnectionString = "";
        public static OleDbConnection CNN = null;
        public static DateTime limiteDate = DateTime.Today;
        public static double limiteJour = 0;
        public static string IdHotel = "4";
        public static int idaffchange = -1;
        public static string SelectionResult1 = "";
        public static string SelectionResult2 = "";
        public static bool fileexist = false;
        #endregion
        #region "SQL Functions"      
        public static DataTable Query(string query)
        {
            query = "set dateformat dmy; " + query;
            DataTable res = null;
            OleDbCommand req = new OleDbCommand();
            req.Connection = CNN;
            req.CommandText = query;
            OleDbDataReader reader = null;
            try
            {
                reader = req.ExecuteReader();
                if (query.ToLower().Contains("select "))
                {
                    res = new DataTable();
                    res.Load(reader);
                    return res;
                }
                res = new DataTable();
                return res;
            }
            catch (Exception e)
            {
                if (Globale.showerrors)
                {
                    MessageBox.Show(e.Message);
                }
                try
                {
                    reader.Close();
                }
                catch
                {
                }
            }
            return null;
        }

        #endregion
        public static bool GetConnectionString()
        {
            ConnectionString = "";
            fileexist = false;
            if (File.Exists(@"cmo.gc"))
            {
                fileexist = true;
            }
            try
            {
                ConnectionString = File.ReadAllText(@"cmo.gc");
            }
            catch
            {

            }

            if (ConnectionString == "")
            {
                using (var dialog = new DataConnectionDialog())
                {
                    DataSource.AddStandardDataSources(dialog);
                    DialogResult userChoice = DataConnectionDialog.Show(dialog);
                    if (userChoice == DialogResult.OK)
                    {
                        ConnectionString = dialog.ConnectionString;
                        if (!fileexist)
                        {
                            File.AppendAllLines(@"cmo.gc", new[] { ConnectionString });
                        }
                        return true;
                    }
                    else
                    {
                        ConnectionString = "";
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
        }
        public static void SeConnecter()
        {
            while (ConnectionString == "")
            {
                GetConnectionString();
                if (ConnectionString != "")
                {
                    CNN = new OleDbConnection(ConnectionString);
                    CNN.Open();
                }
            }
        }
        public static void BackUpBDD(string BackPath)
        {
            string pth = BackPath + "\\Backup.bak";
            Query("backup database[hotel] to disk ='" + pth + "'");
        }
        public static void RestoreBDD()
        {
            Query(@"USE master BACKUP DATABASE Hotel TO DISK='e:\Backup.BAK'");
            //       Microsoft.SqlServer.Management.Smo.Server smoServer =
            //new Server(new ServerConnection("NACER2-PC\\SQLEXPRESS"));

            //       Database db = smoServer.Databases['Hotel'];
            //       string dbPath = Path.Combine(db.PrimaryFilePath, 'MyDataBase.mdf');
            //       string logPath = Path.Combine(db.PrimaryFilePath, 'MyDataBase_Log.ldf');
            //       Restore restore = new Restore();
            //       BackupDeviceItem deviceItem = new BackupDeviceItem('d:\MyDATA.BAK', DeviceType.File);
            //       restore.Devices.Add(deviceItem);
            //       restore.Database = backupDatabaseTo;
            //       restore.FileNumber = restoreFileNumber;
            //       restore.Action = RestoreActionType.Database;
            //       restore.ReplaceDatabase = true;
            //       restore.SqlRestore(smoServer);

            //       db = smoServer.Databases['MyDataBase'];
            //       db.SetOnline();
            //       smoServer.Refresh();
            //       db.Refresh();
        }
        public static bool RestoreIfMinimized(string form)
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Name == form)
                {
                    frm.WindowState = FormWindowState.Normal;
                    frm.BringToFront();
                    frm.Focus();
                    return true;
                }
            }
            return false;
        }
        public static void CloseeIfMinimized(string form)
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Name == form)
                {
                    frm.Close();
                }
            }
        }
        class HardDrive
        {
            private string model = null;
            private string type = null;
            private string serialNo = null;
            public string Model
            {
                get { return model; }
                set { model = value; }
            }
            public string Type
            {
                get { return type; }
                set { type = value; }
            }
            public string SerialNo
            {
                get { return serialNo; }
                set { serialNo = value; }
            }
        }
        
        public static string sqlTxt(string s)
        {
            return (s.Replace("'", "''"));
        }
        public static string GetToday()
        {
            string dt = Query("SELECT CAST(GETDATE() AS DATEtime)").Rows[0][0].ToString();
            return (DateTime.ParseExact(dt, "yyyy-MM-dd", null).ToString());
        }        
        public static String Int2Lettres(double valueDouble)
        {
            //en cas de besoin pour vérifier l'orthographe

            Int32 value = (int)valueDouble;
            Int32 sentimes = (int)((valueDouble - value) * 100);
            System.Text.StringBuilder sbSnt = new System.Text.StringBuilder();
            Int2LettresBase(sbSnt, sentimes);
            String SentimeLettre = sbSnt.ToString() + " centime ";
            Int32 division, reste;
            System.Text.StringBuilder sb;

            try
            {
                //Test l'état null
                if (value == 0) return "zéro Dinar " + SentimeLettre;

                //Décomposition de la valeur en milliards, millions, milliers,...

                sb = new System.Text.StringBuilder();

                //milliard
                division = Math.DivRem(value, 1000000000, out reste);
                if (division > 0)
                {
                    Int2LettresBloc(sb, division);
                    sb.Append(" milliard");
                    if (division > 1) sb.Append('s');
                }

                if (reste > 0)
                {
                    //million
                    value = reste;
                    division = Math.DivRem(value, 1000000, out reste);
                    if (division > 0)
                    {
                        if (sb.Length > 0) sb.Append(' ');
                        Int2LettresBloc(sb, division);
                        sb.Append(" million");
                        if (division > 1) sb.Append('s');
                    }

                    if (reste > 0)
                    {
                        //milliers
                        value = reste;
                        division = Math.DivRem(value, 1000, out reste);
                        if (division > 0)
                        {
                            if (sb.Length > 0) sb.Append(' ');
                            if (division == 1)
                                sb.Append("mille");
                            else
                            {
                                Int2LettresBloc(sb, division);
                                sb.Append(" mille");
                            }
                        }
                        if (reste > 0)
                        {
                            //reste
                            if (sb.Length > 0) sb.Append(' ');
                            Int2LettresBloc(sb, reste);
                        }
                    }
                }
                return sb.ToString() + " Dinar et " + SentimeLettre;
            }
            catch (Exception ex)
            {

                return String.Empty;
            }
            finally
            {
                sb = null;
            }
        }

        /// <summary>
        /// Retourne la conversion d'un bloc de 3 bloc
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static void Int2LettresBloc(System.Text.StringBuilder sb, Int32 value)
        {
            Boolean b_centaines;
            Int32
                division,
                reste;

            try
            {
                division = Math.DivRem(value, 100, out reste);

                //Test si des centaines sont présentes
                if (division > 0)
                {
                    //ajout des centaines à la sortie
                    switch (division)
                    {
                        case 1:
                            {
                                sb.Append("cent");
                                break;
                            }
                        default:
                            {
                                Int2LettresBase(sb, division);
                                sb.Append(" cent");
                                break;
                            }
                    }
                    b_centaines = true;
                }
                else
                {
                    b_centaines = false;
                }

                //Test si il reste des éléments apres les centaines
                if (reste > 0)
                {
                    //Introduction d'un espace si on a intégré des centaines
                    if (b_centaines) sb.Append(' ');
                    //Calcul des dixaines et de leurs reste
                    value = reste;
                    division = Math.DivRem(value, 10, out reste);

                    switch (division)
                    {
                        case 0:
                        case 1:
                        case 7:
                        case 9:
                            {
                                Int2LettresBase(sb, value);
                                break;
                            }
                        default:
                            {
                                Int2LettresBase(sb, division * 10);
                                if (reste > 0)
                                {
                                    if (reste == 1)
                                        sb.Append(" et un");
                                    else
                                    {
                                        sb.Append('-');
                                        Int2LettresBase(sb, reste);
                                    }
                                }

                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
            }
        }

        public static void Int2LettresBase(System.Text.StringBuilder sb, Int32 value)
        {
            switch (value)
            {
                case 0: { sb.Append("zéro"); break; }
                case 1: { sb.Append("un"); break; }
                case 2: { sb.Append("deux"); break; }
                case 3: { sb.Append("trois"); break; }
                case 4: { sb.Append("quatre"); break; }
                case 5: { sb.Append("cinq"); break; }
                case 6: { sb.Append("six"); break; }
                case 7: { sb.Append("sept"); break; }
                case 8: { sb.Append("huit"); break; }
                case 9: { sb.Append("neuf"); break; }
                case 10: { sb.Append("dix"); break; }
                case 11: { sb.Append("onze"); break; }
                case 12: { sb.Append("douze"); break; }
                case 13: { sb.Append("treize"); break; }
                case 14: { sb.Append("quatorze"); break; }
                case 15: { sb.Append("quinze"); break; }
                case 16: { sb.Append("seize"); break; }
                case 17: { sb.Append("dix-sept"); break; }
                case 18: { sb.Append("dix-huit"); break; }
                case 19: { sb.Append("dix-neuf"); break; }
                case 20: { sb.Append("vingt"); break; }
                case 21: { sb.Append("vingt et un"); break; }
                case 22: { sb.Append("vingt-deux"); break; }
                case 23: { sb.Append("vingt-trois"); break; }
                case 24: { sb.Append("vingt-quatre"); break; }
                case 25: { sb.Append("vingt-cinq"); break; }
                case 26: { sb.Append("vingt-six"); break; }
                case 27: { sb.Append("vingt-sept"); break; }
                case 28: { sb.Append("vingt-huit"); break; }
                case 29: { sb.Append("vingt-neuf"); break; }
                case 30: { sb.Append("trente"); break; }
                case 31: { sb.Append("trente et un"); break; }
                case 32: { sb.Append("trente-deux"); break; }
                case 33: { sb.Append("trente-trois"); break; }
                case 34: { sb.Append("trente-quatre"); break; }
                case 35: { sb.Append("trente-cinq"); break; }
                case 36: { sb.Append("trente-six"); break; }
                case 37: { sb.Append("trente-sept"); break; }
                case 38: { sb.Append("trente-huit"); break; }
                case 39: { sb.Append("trente-neuf"); break; }
                case 40: { sb.Append("quarante"); break; }
                case 41: { sb.Append("quarante et un"); break; }
                case 42: { sb.Append("quarante-deux"); break; }
                case 43: { sb.Append("quarante-trois"); break; }
                case 44: { sb.Append("quarante-quatre"); break; }
                case 45: { sb.Append("quarante-cinq"); break; }
                case 46: { sb.Append("quarante-six"); break; }
                case 47: { sb.Append("quarante-sept"); break; }
                case 48: { sb.Append("quarante-huit"); break; }
                case 49: { sb.Append("quarante-neuf"); break; }
                case 50: { sb.Append("cinquante"); break; }
                case 51: { sb.Append("cinquante et un"); break; }
                case 52: { sb.Append("cinquante-deux"); break; }
                case 53: { sb.Append("cinquante-trois"); break; }
                case 54: { sb.Append("cinquante-quatre"); break; }
                case 55: { sb.Append("cinquante-cinq"); break; }
                case 56: { sb.Append("cinquante-six"); break; }
                case 57: { sb.Append("cinquante-sept"); break; }
                case 58: { sb.Append("cinquante-huit"); break; }
                case 59: { sb.Append("cinquante-neuf"); break; }
                case 60: { sb.Append("soixante"); break; }
                case 61: { sb.Append("soixante et un"); break; }
                case 62: { sb.Append("soixante-deux"); break; }
                case 63: { sb.Append("soixante-trois"); break; }
                case 64: { sb.Append("soixante-quatre"); break; }
                case 65: { sb.Append("soixante-cinq"); break; }
                case 66: { sb.Append("soixante-six"); break; }
                case 67: { sb.Append("soixante-sept"); break; }
                case 68: { sb.Append("soixante-huit"); break; }
                case 69: { sb.Append("soixante-neuf"); break; }
                case 70: { sb.Append("soixante-dix"); break; }
                case 71: { sb.Append("soixante et onze"); break; }
                case 72: { sb.Append("soixante-douze"); break; }
                case 73: { sb.Append("soixante-treize"); break; }
                case 74: { sb.Append("soixante-quatorze"); break; }
                case 75: { sb.Append("soixante-quinze"); break; }
                case 76: { sb.Append("soixante-seize"); break; }
                case 77: { sb.Append("soixante-dix-sept"); break; }
                case 78: { sb.Append("soixante-dix-huit"); break; }
                case 79: { sb.Append("soixante-dix-neuf"); break; }
                case 80: { sb.Append("quatre-vingt"); break; }
                case 81: { sb.Append("quatre-vingt et un"); break; }
                case 82: { sb.Append("quatre-vingt-deux"); break; }
                case 83: { sb.Append("quatre-vingt-trois"); break; }
                case 84: { sb.Append("quatre-vingt-quatre"); break; }
                case 85: { sb.Append("quatre-vingt-cinq"); break; }
                case 86: { sb.Append("quatre-vingt-six"); break; }
                case 87: { sb.Append("quatre-vingt-sept"); break; }
                case 88: { sb.Append("quatre-vingt-huit"); break; }
                case 89: { sb.Append("quatre-vingt-neuf"); break; }
                case 90: { sb.Append("quatre-vingt-dix"); break; }
                case 91: { sb.Append("quatre-vingt-onze"); break; }
                case 92: { sb.Append("quatre-vingt-douze"); break; }
                case 93: { sb.Append("quatre-vingt-treize"); break; }
                case 94: { sb.Append("quatre-vingt-quatorze"); break; }
                case 95: { sb.Append("quatre-vingt-quinze"); break; }
                case 96: { sb.Append("quatre-vingt-seize"); break; }
                case 97: { sb.Append("quatre-vingt-dix-sept"); break; }
                case 98: { sb.Append("quatre-vingt-dix-huit"); break; }
                case 99: { sb.Append("quatre-vingt-dix-neuf"); break; }
                case 100: { sb.Append("cent"); break; }
                default: { /*RAS*/ break; }
            }
        }

    }
}
