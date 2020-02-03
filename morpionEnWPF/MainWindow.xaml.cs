using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace morpionEnWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int morpionIndex;
        private readonly string[] morpion = new string[3] { "X", "O", "" };
        private readonly Dictionary<string, SolidColorBrush[]> color = new Dictionary<string, SolidColorBrush[]>();
        List<int> buttonsIndex = new List<int> { 0, 1, 2, 10, 11, 12, 20, 21, 22 };
        private string difficulteJeu = "Moyen";
        private int winner = 0;
        private bool choixMopion = true;

        public MainWindow()
        {
            InitializeComponent();
            color.Add("O", new SolidColorBrush[] { new SolidColorBrush(Colors.Blue), new SolidColorBrush(Colors.Azure) });
            color.Add("X", new SolidColorBrush[] { new SolidColorBrush(Colors.Purple), new SolidColorBrush(Colors.MistyRose) });
            color.Add("", new SolidColorBrush[] { new SolidColorBrush(Colors.Black), new SolidColorBrush(Colors.Beige) });
            color.Add("?", new SolidColorBrush[] { new SolidColorBrush(Colors.Black), new SolidColorBrush(Colors.Beige) });
            NewGame();
        }
        private void ChoixMorpion()
        {
            if (choixMopion)
            {
                morpion[1] = morpion[0] == "O" ? "X" : "O";
                ChoixJ1.Content = morpion[0];
                ChoixJ1.Background = color[morpion[0]][1];
                ChoixJ1.Foreground = color[morpion[0]][0];
                ChoixJ2.Content = morpion[1];
                ChoixJ2.Background = color[morpion[1]][1];
                ChoixJ2.Foreground = color[morpion[1]][0];
            }

        }
        private void buttonClick(Button mybutton)
        {
            if(choixMopion) { choixMopion = false; }
            if (Choix_1.IsEnabled==true)
            {
                Choix_1.IsEnabled = false;
                Choix_2.IsEnabled = false;
            }
            if (winner == 0 && buttonsIndex != null && mybutton != null)
            {
                
                morpionIndex = morpionIndex % 2;
                mybutton.Content = morpion[morpionIndex];
                mybutton.Background = color[morpion[morpionIndex]][1];
                mybutton.Foreground = color[morpion[morpionIndex]][0];
                mybutton.IsEnabled = false;

                // mybutton.Style.Setters. = Color[i % 2];
                //text.Text = $"{mybutton.Name} :\n {morpionIndex}";
                //verifierColumn(mybutton);
                buttonsIndex.Remove(Convert.ToInt32(mybutton.Name.Replace("Button", "")));
                morpionIndex++;
                bool test = false;
                //bool test1 = VerifierColumn(mybutton);
                //bool test2 = VerifierRow(mybutton);
                //bool test3 = VerifierDiagonal(mybutton);
                if (VerifierColumn(mybutton) || VerifierRow(mybutton) || VerifierDiagonal(mybutton))
                //if (test1 || test2 || test3)
                {
                    winner = morpionIndex;
                    Winner.Text = $"      Gagnant : {(morpionIndex == 1 ? NameJ1.Text : NameJ2.Text)}      ";
                    myPopup.IsOpen=true;
                    //Winner.Text = $"{mybutton.Name} :\n {mybutton.Content}";
                    foreach (Button button in GridGame.Children)
                    {

                        //button.IsEnabled = false;
                        button.IsEnabled = !(VerifierColumn(mybutton) || VerifierRow(mybutton) || VerifierDiagonal(mybutton));
                    }
                    scoreJoueur1.Text = $"{Convert.ToInt32(scoreJoueur1.Text) + ((morpionIndex == 1) ? 1 : 0)}";
                    scoreJoueur2.Text = $"{Convert.ToInt32(scoreJoueur2.Text) + ((morpionIndex == 2) ? 1 : 0)}";
                }
                test = morpionIndex % 2 == 1;
                if (winner == 0 && difficulteJeu != "" && morpionIndex % 2 == 1)
                {
                    switch (difficulteJeu)
                    {
                        case "Facile":
                            buttonClick(JeuFacile());
                            break;
                        case "Moyen":
                            buttonClick(JeuMoyen());
                            break;
                        case "Difficile":
                            buttonClick(JeuDifficile());
                            break;
                        case "Expert":
                            buttonClick(JeuExpert());
                            break;
                        default:
                            break;
                    }
                }
            }

        }

        private Button JeuExpert()
        {
            if (buttonsIndex.Count > 7)
            {
                int index = (buttonsIndex.Contains(11)) ? 11 : (buttonsIndex.Contains(0)) ? 0 : 22;
                return (Button)GridGame.FindName("Button" + (index));
            }
            return JeuDifficile();
        }

        private Button JeuDifficile()
        {
            if (buttonsIndex.Count > 0 && buttonsIndex.Count < 8)
            {
                foreach (int index in buttonsIndex)
                {
                    Button btn = (Button)GridGame.FindName("Button" + (index));
                    string btnContent = btn.Content.ToString();
                    btn.Content = morpion[morpionIndex % 2];
                    if (VerifierColumn(btn) || VerifierRow(btn) || VerifierDiagonal(btn))
                    {
                        return btn;
                    }
                    else
                    {
                        btn.Content = btnContent;
                    }
                }
                foreach (int index in buttonsIndex)
                {
                    Button btn = (Button)GridGame.FindName("Button" + (index));
                    string btnContent = btn.Content.ToString();
                    btn.Content = morpion[(morpionIndex+1) % 2];
                    if (VerifierColumn(btn) || VerifierRow(btn) || VerifierDiagonal(btn))
                    {
                        return btn;
                    }
                    else
                    {
                        btn.Content = btnContent;
                    }
                }
            }
            return JeuFacile();
        }

        private Button JeuMoyen()
        {
            if (buttonsIndex.Count > 0 && buttonsIndex.Count < 7)
            {
                foreach (int index in buttonsIndex)
                {
                    Button btn = (Button)GridGame.FindName("Button" + (index));
                    string btnContent = btn.Content.ToString();
                    btn.Content = morpion[morpionIndex % 2];
                    if (VerifierColumn(btn) || VerifierRow(btn) || VerifierDiagonal(btn))
                    {
                        return btn;
                    }
                    else
                    {
                        btn.Content = btnContent;
                    }
                }
            }
            return JeuFacile();
        }

        private Button JeuFacile()
        {
            if (buttonsIndex.Count > 0)
            {
                var rand = new Random();
                int buttonIndex = rand.Next(buttonsIndex.Count);
                Button btn = (Button)GridGame.FindName("Button" + (buttonsIndex[buttonIndex]));
                return btn;

            }
            return null;
        }

        private void NewGame()
        {
            ChoixJeu.Content = (difficulteJeu != "") ? "Jeu " + difficulteJeu : "Deux joueurs ";
            morpionIndex = 0;
            winner = 0;
            buttonsIndex = new List<int> { 0, 1, 2, 10, 11, 12, 20, 21, 22 };
            Winner.Text = "";
            choixMopion = true;

            Choix_1.IsEnabled = true;
            Choix_2.IsEnabled = true;
            foreach (Button button in GridGame.Children)
            {
                button.Content = morpion[2];
                button.Background = color[morpion[2]][1];
                button.Foreground = color[morpion[2]][0];
                button.IsEnabled = true;
            }

        }


        private bool VerifierColumn(Button mybutton)
        {
            int indexCase = Convert.ToInt32(mybutton.Name.Replace("Button", ""));
            int indexColumn = (indexCase - indexCase % 10) / 10;
            Button btn1 = (Button)GridGame.FindName("Button" + (indexColumn * 10));
            Button btn2 = (Button)GridGame.FindName("Button" + (indexColumn * 10 + 1));
            Button btn3 = (Button)GridGame.FindName("Button" + (indexColumn * 10 + 2));
            int somme = (btn1.Content.ToString() == morpion[0]) ? 0 : (btn1.Content.ToString() == morpion[1]) ? 1 : 4;
            somme += (btn2.Content.ToString() == morpion[0]) ? 0 : (btn2.Content.ToString() == morpion[1]) ? 1 : 4;
            somme += (btn3.Content.ToString() == morpion[0]) ? 0 : (btn3.Content.ToString() == morpion[1]) ? 1 : 4;
            //Winner.Text = "C : " + somme + " - " + (somme < 4 && somme % 3 == 0).ToString();
            //Winner.Text += "\n" + btn1.Content;
            //Winner.Text += " - " + btn2.Content;
            //Winner.Text += " - " + btn3.Content;
            if (somme < 4 && somme % 3 == 0)
            {
                if (winner != 0)
                {
                    btn1.Foreground = new SolidColorBrush(Colors.Gold);
                    btn2.Foreground = new SolidColorBrush(Colors.Gold);
                    btn3.Foreground = new SolidColorBrush(Colors.Gold);
                }
                return true;
            }


            return false;
        }


        private bool VerifierRow(Button mybutton)
        {
            int indexCase = Convert.ToInt32(mybutton.Name.Replace("Button", ""));
            int indexRow = indexCase % 10;
            Button btn1 = (Button)GridGame.FindName("Button" + indexRow);
            Button btn2 = (Button)GridGame.FindName("Button" + (indexRow + 10));
            Button btn3 = (Button)GridGame.FindName("Button" + (indexRow + 20));
            int somme = (btn1.Content.ToString() == morpion[0]) ? 0 : (btn1.Content.ToString() == morpion[1]) ? 1 : 4;
            somme += (btn2.Content.ToString() == morpion[0]) ? 0 : (btn2.Content.ToString() == morpion[1]) ? 1 : 4;
            somme += (btn3.Content.ToString() == morpion[0]) ? 0 : (btn3.Content.ToString() == morpion[1]) ? 1 : 4;
            //Winner.Text = "R : " + somme + " - " + (somme < 4 && somme % 3 == 0).ToString();
            //Winner.Text += "\n" + btn1.Content;
            //Winner.Text += " - " + btn2.Content;
            //Winner.Text += " - " + btn3.Content;
            if (somme < 4 && somme % 3 == 0)
            {
                if (winner != 0)
                {
                    btn1.Foreground = new SolidColorBrush(Colors.Gold);
                    btn2.Foreground = new SolidColorBrush(Colors.Gold);
                    btn3.Foreground = new SolidColorBrush(Colors.Gold);
                }
                return true;
            }


            return false;
        }

        //Diagonal
        private bool VerifierDiagonal(Button mybutton)
        {
            int indexCase = Convert.ToInt32(mybutton.Name.Replace("Button", ""));
            int somme = 4;
            if ((new int[] { 0, 11, 22 }).Contains(indexCase))
            {
                somme = (Button0.Content.ToString() == morpion[0]) ? 0 : (Button0.Content.ToString() == morpion[1]) ? 1 : 4;
                somme += (Button11.Content.ToString() == morpion[0]) ? 0 : (Button11.Content.ToString() == morpion[1]) ? 1 : 4;
                somme += (Button22.Content.ToString() == morpion[0]) ? 0 : (Button22.Content.ToString() == morpion[1]) ? 1 : 4;
                if (winner!=0 && somme < 4 && somme % 3 == 0)
                {
                    Button0.Foreground = new SolidColorBrush(Colors.Gold);
                    Button11.Foreground = new SolidColorBrush(Colors.Gold);
                    Button22.Foreground = new SolidColorBrush(Colors.Gold);
                }

                //Winner.Text += "\nD1 :" + somme + " - " + (somme < 4 && somme % 3 == 0).ToString();
            }
            if ((somme > 4 || somme % 3 != 0) && (new int[] { 2, 11, 20 }).Contains(indexCase))
            {
                somme = (Button2.Content.ToString() == morpion[0]) ? 0 : (Button2.Content.ToString() == morpion[1]) ? 1 : 4;
                somme += (Button11.Content.ToString() == morpion[0]) ? 0 : (Button11.Content.ToString() == morpion[1]) ? 1 : 4;
                somme += (Button20.Content.ToString() == morpion[0]) ? 0 : (Button20.Content.ToString() == morpion[1]) ? 1 : 4;
                if (winner != 0 && somme < 4 && somme % 3 == 0)
                {
                    Button20.Foreground = new SolidColorBrush(Colors.Gold);
                    Button11.Foreground = new SolidColorBrush(Colors.Gold);
                    Button2.Foreground = new SolidColorBrush(Colors.Gold);
                }
                //Winner.Text += "\nD2 : " + somme + " - " + (somme < 4 && somme % 3 == 0).ToString();
            }
            return somme < 4 && somme % 3 == 0;
        }

        // 
        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            buttonClick(Button0);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            buttonClick(Button1);
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            buttonClick(Button2);
        }

        private void Button10_Click(object sender, RoutedEventArgs e)
        {
            buttonClick(Button10);
        }
        private void Button20_Click(object sender, RoutedEventArgs e)
        {
            buttonClick(Button20);
        }

        private void Button11_Click(object sender, RoutedEventArgs e)
        {
            buttonClick(Button11);
        }
        private void Button21_Click(object sender, RoutedEventArgs e)
        {
            buttonClick(Button21);
        }

        private void Button12_Click(object sender, RoutedEventArgs e)
        {
            buttonClick(Button12);
        }
        private void Button22_Click(object sender, RoutedEventArgs e)
        {
            buttonClick(Button22);
        }

        private void Choix_1_Click(object sender, RoutedEventArgs e)
        {
            morpion[0] = "O";
            ChoixMorpion();
        }

        private void Choix_2_Click(object sender, RoutedEventArgs e)
        {
            morpion[0] = "X";
            ChoixMorpion();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void MenuItem_Click_Facile(object sender, RoutedEventArgs e)
        {
            difficulteJeu = "Facile";
        }

        private void MenuItem_Click_Moyen(object sender, RoutedEventArgs e)
        {
            difficulteJeu = "Moyen";
        }

        private void MenuItem_Click_Difficile(object sender, RoutedEventArgs e)
        {
            difficulteJeu = "Difficile";
        }

        private void MenuItem_Click_Expert(object sender, RoutedEventArgs e)
        {
            difficulteJeu = "Expert";
        }
        private void MenuItem_Click_2playrs(object sender, RoutedEventArgs e)
        {
            difficulteJeu = "";
        }

        private void IniialiserScore_Click(object sender, RoutedEventArgs e)
        {
            scoreJoueur1.Text = "0";
            scoreJoueur2.Text = "0";
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Close this window
            this.Close();
        }

        private void CloseMyPopup_Click(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = false; 
        }
    }
}
