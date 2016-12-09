using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworksApi.TCP.CLIENT;

namespace AdvanceClient
{
    public delegate void UpdateText(string txt);
    public partial class Form1 : Form
    {
        Client client;
        public Form1()
        {
            InitializeComponent();
        }
      
        private void Textdegis(string txt)// bu fonksiyona gelen mesajları ekrana(TextBox1'e) yazar
        {
            if(textBox1.InvokeRequired)//gelen metin suncuudan geldiyse if'e gir
            {
                Invoke(new UpdateText(Textdegis), new object[] { txt });//sunucdan gelen mesajı yazdır
            }
            else
            {
               /*Sunucu tarafından kullanıcı atılırsa bu kısım çalışır*/
                if (txt== "Sunucuya bağlantı sağlandı!")//a true ise if'e gir
                {
                    button3.Enabled = false;//bağlan butonunu pasif yap
                    button4.Enabled = true;//bağlantıyı kes butonunu aktif yap
                }
                else if(txt== "Sunucu ile bağlantı kesildi!")
                {
                    button3.Enabled = true;
                    button4.Enabled = false;
                }/*Sunucu tarafından kullanıcı atılırsa bu kısım çalışır*/
                else if (txt == "Sunucuya bağlantı sağlanamadı!")//İstemci sunucuya bağlanamazsa
                {
                    button3.Enabled = true;
                    button4.Enabled = false;
                }
                textBox1.Text += txt + "\r\n";//gelen yazıyı textboxa ekleyerek bi satır alta geçer yeni gelen her metin bi alt satıra yazılır
            textBox1.SelectionStart = textBox1.Text.Length;//Sürekli TextBox'ın sonunu göster
            textBox1.ScrollToCaret();//Sürekli TextBox'ın sonunu göster
            }
        }

        private void client_OnDataReceived(object Sender, ClientReceivedArguments R)//Sunucudan mesaj geldiğince çalışacak fonksiyon
        {
            Textdegis(R.ReceivedData);//Sunucudan gelen mesajı ekrana yazan fonksıyona gelen mesajı gönderır
        }

        private void client_OnClientError(object Sender, ClientErrorArguments R)//Sunucuya bağlanılamadığı durumda çalışacak fonksiyon
        {
            Textdegis("Sunucuya bağlantı sağlanamadı!");//bağlantı sağlanamdığında ekrana uyarı verir
         
        }

        private void client_OnClientDisconnected(object Sender, ClientDisconnectedArguments R)//sunucu ile bağlantı kesildiğinde çalışacak fonskiyon
        {

            Textdegis("Sunucu ile bağlantı kesildi!");//bağlantı kesildiğinde ekrana bilgiyi yazar
        }

        private void client_OnclientConnecting(object Sender, ClientConnectingArguments R)// sunucuya bağlanılırken çalışacak fonksiyon
        {
            Textdegis("Sunucu bağlantısı kontrol ediliyor!");//bağlanırken akrana durumu yazar
        }

        private void client_OnClientConnected(object Sender, ClientConnectedArguments R)//bağlantı başarılı ise çalışacak fonksiyon
        {

            Textdegis("Sunucuya bağlantı sağlandı!"); // bağlantının başaralı olduğunu istemci ekranına yazar
        }

        private void button1_Click(object sender, EventArgs e)//Mesaj gönder butonuna basıldığında çalışacak fonksiyon
        {
            if(client !=null&& client.IsConnected)// istemci null değil ve bağlıysa if'e gir
            {
                client.Send(textBox2.Text);//mesajı gönder
                textBox2.Clear();//mesaj alanını temizle
                textBox1.SelectionStart = textBox1.Text.Length;//Sürekli TextBox'ın sonunu göster
                textBox1.ScrollToCaret();//Sürekli TextBox'ın sonunu göster
            }
            textBox1.SelectionStart = textBox1.Text.Length;//Sürekli TextBox'ın sonunu göster
            textBox1.ScrollToCaret();//Sürekli TextBox'ın sonunu göster
        }

        private void button2_Click(object sender, EventArgs e)//gelen mesaj ekranımızı temizleme fonksiyonu
        {
            textBox1.Clear();//gelen mesaj ekranını temizler
        }

        private void button3_Click(object sender, EventArgs e)//Bağlan butonuna basıldığında çalıaşcak fonksiyon
        {

            if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")//textboxlar boş değilse if'e gir
            {
                this.Height = 530; //Formun Yüksekliği ni 550 ye çıkar
                client = new Client();// yeni İsemci oluştur
                client.ClientName = textBox3.Text;// girilen kullanıcı adını al
                client.ServerIp = textBox4.Text;// girilen sunucu ip sini al
                client.ServerPort = textBox5.Text;// girilen sunucu port noasunu al
                client.OnClientConnected += new OnClientConnectedDelegate(client_OnClientConnected);//bağlantı sağlandığında çalışacak fonksiyonu tanımlama
                client.OnClientConnecting += new OnClientConnectingDelegate(client_OnclientConnecting);//bağlanma aşamasında çalışacak fonksiyonu tanımlama
                client.OnClientDisconnected += new OnClientDisconnectedDelegate(client_OnClientDisconnected); //bağlantı kesildiğinde çalışacak fonksiyon
                client.OnClientError += new OnClientErrorDelegate(client_OnClientError); //hata durumunda çalışacak fonksiyonu tanımlama
                client.OnDataReceived += new OnClientReceivedDelegate(client_OnDataReceived);//mesaj alma fonksiyonu tanımlama
                client.Connect();// istemciyi başlat
                button3.Enabled = false;//başlat butonunu pasif yap
                button4.Enabled = true;//bağlantıyı kes butonunu aktif yap
            }

            else//textboxlar boş ise
            {
                MessageBox.Show("Lütfen ilgili alanları doldurunuz!", "İstemci Bilgilendirme Penceresi", MessageBoxButtons.OK, MessageBoxIcon.Information);//  uyarı mesajı ver
            }

            textBox1.SelectionStart = textBox1.Text.Length; //Sürekli TextBox'ın sonunu göster
            textBox1.ScrollToCaret();//Sürekli TextBox'ın sonunu göster
        }

        private void button4_Click(object sender, EventArgs e)//Bağlantıyı kes butonuna basıldığında çalışacak fonksiyon
        {
            client.Disconnect(); //istemci bağlantısını kesmeye 
            button3.Enabled = true; //bağlan butonunu aktif yap
            button4.Enabled = false;//bağlantı kes butonunu pasif yap
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)//mesaj yazıp 'enter'a bastıgımızda mesajın gitmesi fonksiyonu
        {
            if (client != null && client.IsConnected && e.KeyCode==Keys.Enter)//istemci null değilse istemci bağlıysa ve entera basıldıysa  if'e gir
            {
                client.Send(textBox2.Text); //mesajı gönder
                textBox2.Clear();//mesaj alanını temizle
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)//Form kapatıltığında çalışacak fonksiyon
        {
            System.Environment.Exit(System.Environment.ExitCode);//uygulamadan çıkmak için Kullanılır
        }
        
    }
}
