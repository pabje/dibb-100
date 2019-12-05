using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace DIBB_100
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {

        }

        private void Btnabrir_Click(object sender, EventArgs e)
        {
            OpenFileDialog buscar = new OpenFileDialog();
            if(buscar.ShowDialog() == DialogResult.OK)
            {
                SqlConnection cn;
                //string con;
                //tratar archivo csv
                string dir = buscar.FileName;
                int numfac,f;
                string sernum;
                int soptype;
                if (Path.GetExtension(dir) == ".csv")
                {
                    //trato el archivo .csv
                    using (TextFieldParser parser = new TextFieldParser(@dir))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(";");
                        f = 0;
                        //SqlConnection cn = new SqlConnection("Persist Security Info=False;User ID=FECOL1;Password=fecol1;Initial Catalog=COL10;Server=10.1.1.22");
                        cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DIBB100"].ConnectionString);
                        cn.Open();
                        while (!parser.EndOfData)
                        {
                            //Processing row
                            string[] fields = parser.ReadFields();
                            if (f > 0)
                            {
                                soptype = 3;
                                numfac = int.Parse(fields[1]);                                    
                                sernum = fields[5] + fields[6];                                
                                try
                                {
                                    //using (SqlConnection cn = new SqlConnection("Persist Security Info=False;User ID=sa;Password=1234;Initial Catalog=getty;Server=LAPTOP-0H8OVDMS"))
                                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZARNUMEROFISCAL", cn);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@SOPTYPE", soptype);
                                    cmd.Parameters.AddWithValue("@NUMFAC", numfac);
                                    cmd.Parameters.AddWithValue("@SERNUM", sernum);
                                    SqlParameter MENS = new SqlParameter("@MENS", SqlDbType.VarChar,200);                                    
                                    MENS.Direction = ParameterDirection.Output;
                                    cmd.Parameters.Add(MENS);
                                    cmd.ExecuteScalar();
                                    //cmd.ExecuteNonQuery();
                                    MessageBox.Show(MENS.Value.ToString() + ": " + numfac);
                                    //MessageBox.Show(MENS.Value.ToString() + ": " + numfac);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }                                
                            }
                            f++;
                        }
                        cn.Close();
                    }
                }
                else
                    MessageBox.Show("Error: no abrio un archivo .csv");
            }
        }
    }
}
