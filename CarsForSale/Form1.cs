using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CarsForSale
{
    public partial class Form1 : Form
    {
        List<SimpleModel> models;
        List<Manufacturer> manus;
        int selectedId;
        
        public Form1()
        {
            InitializeComponent();
        }

        //Loads form and gets the list of models from server and adds them to listbox.
        private async void Form1_Load(object sender, EventArgs e)
        {
            models = await LoadModels();
            if (models != null)
            {
                foreach (SimpleModel m in models)
                {
                    listBox1.Items.Add(m.Name + ", " + m.ManufacturerName);
                }
            }
        }

        //GET list of models and manufacturers from server.
        private async Task<List<SimpleModel>> LoadModels()
        {
            models = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62241/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Models");
                if (response.IsSuccessStatusCode)
                {
                    models = await response.Content.ReadAsAsync<List<SimpleModel>>();
                }

                response = await client.GetAsync("api/Manufacturers");
                if (response.IsSuccessStatusCode)
                {
                    manus = await response.Content.ReadAsAsync<List<Manufacturer>>();
                }
            }
            return models;
        }

        //When selected listbox item changes it clear the fields, finds the id of the selected item
        //and gets its details from server and sets them to fields accordingly.
        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbModel.Clear();
            tbManufacturer.Clear();
            tbYear.Clear();
            tbPrice.Clear();
            cbFuelType.SelectedIndex = -1;
        
            if(listBox1.SelectedIndex != -1)
            {
                selectedId = models[listBox1.SelectedIndex].Id;
        
                Model model = await LoadModel();
        
                if (model != null)
                {
                    tbModel.Text = model.Name;
                    tbManufacturer.Text = models[listBox1.SelectedIndex].ManufacturerName;
                    tbYear.Text = model.Year.ToString();
                    tbPrice.Text = model.Price.ToString();
                    if (model.FuelType.ToString().Equals("Gas"))
                    {
                        cbFuelType.SelectedIndex = 0;
                    }
                    else if (model.FuelType.ToString().Equals("Diesel"))
                    {
                        cbFuelType.SelectedIndex = 1;
                    }
                }
            }
        }

        //GET detailed model from server.
        private async Task<Model> LoadModel()
        {
            Model model = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62241/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string u = "api/Models/" + selectedId.ToString();
                HttpResponseMessage response = await client.GetAsync(u);
                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<Model>();
                }
            }
            return model;
        }

        //Clears the fields and selection.
        private void btnClear_Click(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            tbModel.Clear();
            tbManufacturer.Clear();
            tbYear.Clear();
            tbPrice.Clear();
            cbFuelType.SelectedIndex = -1;
        }

        //Create new model based on the information on the fields.
        //Add the new model to the server and clear fields.
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            Model model = new Model();
            model.Id = models[models.Count() - 1].Id + 1;
            model.Name = tbModel.Text;
            try 
	        {
                model.Year = Int32.Parse(tbYear.Text.ToString());
	        }
	        catch (Exception)
	        {
                MessageBox.Show("Give year in numbers!");
                return;
	        }
            try
            {
                model.Price = Decimal.Parse(tbPrice.Text.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Give year in numbers!");
                return;
            }
            model.FuelType = cbFuelType.SelectedText;
            for (int i = 0; i < manus.Count; i++)
			{
			    if(tbManufacturer.Text.Equals(manus[i].Name))
                {
                    model.ManufacturerId = manus[i].Id;
                }   
			}

            await AddModel(model);

            tbModel.Clear();
            tbManufacturer.Clear();
            tbYear.Clear();
            tbPrice.Clear();
            cbFuelType.SelectedIndex = -1;
        }
        
        //POST new model to the server and get new updated list of models and update the listbox.
        private async Task AddModel(Model m)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62241/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Models", m);
                if (response.IsSuccessStatusCode)
                {
                    Uri modelUrl = response.Headers.Location;
                }

                models = null;
                response = await client.GetAsync("api/Models");
                if (response.IsSuccessStatusCode)
                {
                    models = await response.Content.ReadAsAsync<List<SimpleModel>>();

                    if (models != null)
                    {
                        listBox1.Items.Clear();
                        foreach (SimpleModel model in models)
                        {
                            listBox1.Items.Add(model.Name + ", " + model.ManufacturerName);
                        }
                    }
                }
            }
        }

        //Create new model with the selected id and update it in the server. Clear the fields.
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            Model model = new Model();
            model.Id = selectedId;
            model.Name = tbModel.Text;
            try
            {
                model.Year = Int32.Parse(tbYear.Text.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Give year in numbers!");
                return;
            }
            try
            {
                model.Price = Decimal.Parse(tbPrice.Text.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Give year in numbers!");
                return;
            }
            model.FuelType = cbFuelType.Text;
            for (int i = 0; i < manus.Count; i++)
            {
                if (tbManufacturer.Text.Equals(manus[i].Name))
                {
                    model.ManufacturerId = manus[i].Id;
                }
            }

            await EditModel(model);

            tbModel.Clear();
            tbManufacturer.Clear();
            tbYear.Clear();
            tbPrice.Clear();
            cbFuelType.SelectedIndex = -1;
        }

        //PUT edited model to the server and get updated list of model and update them to the listbox.
        private async Task EditModel(Model m)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62241/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string uri = "api/Models/" + m.Id.ToString();
                HttpResponseMessage response = await client.PutAsJsonAsync(uri, m);

                models = null;
                response = await client.GetAsync("api/Models");
                if (response.IsSuccessStatusCode)
                {
                    models = await response.Content.ReadAsAsync<List<SimpleModel>>();

                    if (models != null)
                    {
                        listBox1.Items.Clear();
                        foreach (SimpleModel model in models)
                        {
                            listBox1.Items.Add(model.Name + ", " + model.ManufacturerName);
                        }
                    }
                }
            }
        }

        //Delete the selected model from the server and clear the fields.
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteModel(selectedId);

            tbModel.Clear();
            tbManufacturer.Clear();
            tbYear.Clear();
            tbPrice.Clear();
            cbFuelType.SelectedIndex = -1;
        }

        //DELETE the model from the server and get the updated list of models and update them to the listbox.
        private async Task DeleteModel(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62241/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string uri = "api/Models/" + id.ToString();
                HttpResponseMessage response = await client.DeleteAsync(uri);

                models = null;
                response = await client.GetAsync("api/Models");
                if (response.IsSuccessStatusCode)
                {
                    models = await response.Content.ReadAsAsync<List<SimpleModel>>();

                    if (models != null)
                    {
                        listBox1.Items.Clear();
                        foreach (SimpleModel model in models)
                        {
                            listBox1.Items.Add(model.Name + ", " + model.ManufacturerName);
                        }
                    }
                }
            }
        }
    }
}
