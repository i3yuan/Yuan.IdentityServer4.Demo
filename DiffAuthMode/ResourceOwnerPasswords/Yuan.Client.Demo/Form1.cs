using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yuan.Client.Demo
{
    public partial class Form1 : Form
    {
        TokenResponse tokenResponse = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           

        }

        private void getToken_Click(object sender, EventArgs e)
        {
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync(this.txtIdentityServer.Text).Result;
            if (disco.IsError)
            {
                this.tokenList.Text = disco.Error;
                return;
            }
            //请求token
            tokenResponse = client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId =this.txtClientId.Text,
                ClientSecret = this.txtClientSecret.Text,
                Scope = this.txtApiScopes.Text,
                UserName=this.txtUserName.Text,
                Password=this.txtPassword.Text
            }).Result;

            if (tokenResponse.IsError)
            {
                this.tokenList.Text = disco.Error;
                return;
            }
            this.tokenList.Text = JsonConvert.SerializeObject(tokenResponse.Json);
            this.txtToken.Text = tokenResponse.AccessToken;
        }

        private void getApi_Click(object sender, EventArgs e)
        {

            //调用认证api
            if (string.IsNullOrEmpty(txtToken.Text))
            {
                MessageBox.Show("token值不能为空");
                return;
            }
            var apiClient = new HttpClient();
            //apiClient.SetBearerToken(tokenResponse.AccessToken);
            apiClient.SetBearerToken(this.txtToken.Text);

            var response = apiClient.GetAsync(this.txtApi.Text).Result;
            if (!response.IsSuccessStatusCode)
            {
                this.resourceList.Text = response.StatusCode.ToString();
            }
            else
            {
                this.resourceList.Text = response.Content.ReadAsStringAsync().Result;
            }

        }
    }
}
