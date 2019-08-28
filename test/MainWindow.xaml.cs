using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DbConnect dbconn = new DbConnect();//Создали экземпляр класса 
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }
        static bool isChanged = false;
        static double[] result = new double[13];//Массив для хранения результаты каждой М
        
        static double NoQuestResult = 0; //Нужна для НЕТ ОТВЕТА
        public void Start()
        {
            DataTable dt1 = dbconn.GetTable("questions");// В переменную dt1 загнали таблицу questions
         


            WindowState = WindowState.Maximized;// Окно на весь экран
            DataTable dt = dbconn.GetTable("Topics");

            var tnames = dt.AsEnumerable().Where(c => c.Field<string>("name") != ""); //Выборка из таблицы Topics по полю name
            Topics.Children.Clear(); //Очистим панельку

            foreach (var t in tnames)
            {
                bool isFinished = false;// Переменная для проверки закончить блок

                Border b = new Border(); //Создаем элемент для каждой таблицы М типа border
                b.BorderThickness = new Thickness(0.5); 
                b.BorderBrush = Brushes.Black;
                b.Margin = new Thickness(0, 10, 0, 0);
                b.BorderThickness = new Thickness(1);
                b.CornerRadius = new CornerRadius(5);
                b.Background = new SolidColorBrush(Color.FromRgb(20, 100, 229));
                b.Width = 500;
                b.Height = 60;
                
                StackPanel stp = new StackPanel();// Панель чтобы засунуть текстбокс
                
                TextBlock txb = new TextBlock();// Текст внутри border
                txb.Margin = new Thickness(5, 5, 0, 0);
                txb.FontFamily = new FontFamily("Arial");
                txb.FontSize = 14;
                txb.Foreground = Brushes.White;
                txb.TextWrapping = TextWrapping.Wrap;
                txb.Text = t.Field<string>("name");//Берем название таблицы


                b.Tag = t.Field<string>("table_name").ToString();// в свойство Tag бордера присврим значение m1... m13

                Topics.Children.Add(b);// в стекпанель топиков добавляем панель бордер
                stp.Children.Add(txb);// в стекпанель добаляем текстбокс
                b.Child = stp;// в бордер добавляем стекпанель
                b.MouseEnter += (s, j) =>      // событие на наведение курсора на бордер
                {
                    Mouse.OverrideCursor = Cursors.Hand;
                    if(!isFinished) b.Background = new SolidColorBrush(Color.FromRgb(6, 55, 135));
                    txb.Foreground = new SolidColorBrush(Color.FromRgb(198, 204, 211));
                    txb.FontStyle = FontStyles.Italic;
                };
                b.MouseLeave += (s, j) => // событие на удаление курсора из бордера
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    if (!isFinished) b.Background = new SolidColorBrush(Color.FromRgb(20, 100, 229));
                    txb.Foreground = Brushes.White;
                    txb.FontStyle = FontStyles.Normal;
                };
                b.PreviewMouseUp += (s, j) => // событие на нажатие Топика
                {
                    if (isChanged == false) //если не закончен блок данного требования
                    {
                        Questions.Children.Clear();//Очистим левую панельку



                        dt = dbconn.GetTable("questions");
                        var tquestions = dt.AsEnumerable().Where(c => c.Field<string>("table") == b.Tag.ToString());//Берем из БД строку с определенным полем связи и присваиваем в tquestions


                       


                        int count = dt.AsEnumerable().Where(c => c.Field<string>("table") == b.Tag.ToString()).Count();//  переменной count присваиваем количество вопросов для данной категории(требования)
                        double[] array = new double[count];//  Массив для ведения счета каждого вопроса
                          
                        int answerscount = 0;//  Переменная для того чтобы нельзя было выйти из блока не закончив его
                        bool[] noquest = new bool[count];//  массив для Нет ответа
                        TextBlock header = new TextBlock();
                        header.FontSize = 24;
                        header.FontFamily = new FontFamily("Arial");
                        header.Text = txb.Text;
                        header.TextAlignment = TextAlignment.Center;
                        header.TextWrapping = TextWrapping.Wrap;
                        Questions.Children.Add(header);


                        foreach (var q in tquestions) //  для каждого вопроса создаем дизайн
                        {
                            



                            Border b1 = new Border();
                            b1.BorderThickness = new Thickness(0.5);
                            b1.BorderBrush = Brushes.Black;
                            b1.Margin = new Thickness(0, 10, 0, 0);
                            b1.BorderThickness = new Thickness(1);
                            b1.CornerRadius = new CornerRadius(5);
                            b1.Background = new SolidColorBrush(Color.FromRgb(20, 100, 229));
                            b1.Width = 700;
                            b1.Height = double.NaN;


                            StackPanel stp1 = new StackPanel();
                            stp1.Orientation = Orientation.Vertical;
                            TextBlock txb1 = new TextBlock();
                            txb1.Margin = new Thickness(5, 5, 5, 5);
                            txb1.FontFamily = new FontFamily("Arial");
                            txb1.FontSize = 14;
                            txb1.Foreground = Brushes.White;
                            txb1.TextWrapping = TextWrapping.Wrap;
                            txb1.Text = q.Field<string>("question");

                            StackPanel answerstp = new StackPanel();
                            answerstp.Orientation = Orientation.Horizontal;
                            answerstp.Background = new SolidColorBrush(Color.FromRgb(198, 204, 211));


                            string topicsansw = q.Field<string>("table");

                            RadioButton yes = new RadioButton();
                            RadioButton no = new RadioButton();
                            RadioButton idk = new RadioButton();
                            yes.Margin = no.Margin = idk.Margin = new Thickness(0, 3, 5, 0);
                            yes.FontSize = no.FontSize = idk.FontSize = 14;
                            yes.VerticalContentAlignment = no.VerticalContentAlignment = idk.VerticalContentAlignment = VerticalAlignment.Center;

                            yes.GroupName = no.GroupName = idk.GroupName = topicsansw + "_" + answerscount;
                            yes.Content = "Да";
                            no.Content = "Нет";
                            idk.Content = "Нет ответа";
                            int numberofanswer = answerscount;
                            //idk.IsChecked = true;

                            //  Нажатие радиокнопки с ответом Да
                            yes.Click += (l, k) =>
                            {
                                isChanged = true;
                                array[numberofanswer] = 1;
                                noquest[numberofanswer] = false;
                            };
                            //  Нажатие радиокнопки с отвеотом Нет
                            no.Click += (l, k) =>
                            {
                                isChanged = true;
                                array[numberofanswer] = 0;
                                noquest[numberofanswer] = false;
                            };
                            //  Нажатие радиокнопки с нажатием Нет ответа
                            idk.Click += (l, k) =>
                            {
                                isChanged = true;
                                noquest[numberofanswer] = true;
                               
                            };
                            answerstp.Children.Add(yes);
                            answerstp.Children.Add(no);
                            answerstp.Children.Add(idk);

                            Questions.Children.Add(b1);
                            stp1.Children.Add(txb1);
                            stp1.Children.Add(answerstp);
                            b1.Child = stp1;
                            answerscount++;
                        }
                        if (answerscount != 0)
                        {
                            // Создаем блок "ЗАКОНЧИТЬ БЛОК"
                            Border finishblock_border = new Border();
                            finishblock_border.Width = 200;
                            finishblock_border.Height = 40;
                            finishblock_border.BorderThickness = new Thickness(1);
                            finishblock_border.BorderBrush = Brushes.Black;
                            finishblock_border.Margin = new Thickness(0, 50, 0, 0);
                            finishblock_border.BorderThickness = new Thickness(1);
                            finishblock_border.CornerRadius = new CornerRadius(5);
                            finishblock_border.Background = new SolidColorBrush(Color.FromRgb(148, 160, 181));

                            TextBlock finishtxb = new TextBlock();
                            finishtxb.Margin = new Thickness(40, 10, 0, 0);
                            finishtxb.FontFamily = new FontFamily("Arial");
                            finishtxb.FontSize = 16;
                            finishtxb.Foreground = Brushes.Black;
                            finishtxb.TextWrapping = TextWrapping.Wrap;
                            finishtxb.Text = "Закончить блок";

                            finishblock_border.Child = finishtxb;

                            Questions.Children.Add(finishblock_border);

                            //Наведение курсора на Закончить блок
                            finishblock_border.MouseEnter += (l, k) =>
                            {
                                Mouse.OverrideCursor = Cursors.Hand;
                                finishtxb.FontStyle = FontStyles.Italic;
                            };

                            finishblock_border.MouseLeave += (l, k) =>
                            {
                                Mouse.OverrideCursor = Cursors.Arrow;
                                finishtxb.FontStyle = FontStyles.Normal;
                            };
                            // НАжатие на Закончить блок
                            finishblock_border.PreviewMouseUp += (l, k) =>
                            {



                                double[] arrayfactors = dt.AsEnumerable()
                                       .Where(c => c.Field<string>("table") == b.Tag.ToString())
                                       .Select(row => Convert.ToDouble(row.Field<double>("factor"), System.Globalization.CultureInfo.InvariantCulture)).ToArray();// в массив присваеиваем значеня коэффицентов вопросов 



                                isChanged = false;
                                int i = 0;
                                int tablenum = Convert.ToInt32(b.Tag.ToString().Substring(1,b.Tag.ToString().Length-1));

                                bool isNoquest = false;

                                string numbersofNOQUEST = "";


                                // Набираем в строку номера вопросов с ответом НЕТ ОТВЕТА
                                foreach(var p in noquest)
                                {
                                    if (p)
                                    {
                                        isNoquest = true;
                                        numbersofNOQUEST += i.ToString() + ",";
                                    }
                                    i++;
                                }
                                                              
                                string[] numNoquest = numbersofNOQUEST.Split(new Char[] {','});
                                result[tablenum-1] = 0;
                                NoQuestResult = 0;
                                // СЧитаем результаты если выбрали хоть раз НЕт ответа
                                if (isNoquest)
                                {
                                    
                                    foreach(string st in numNoquest)
                                    {
                                        
                                        if (st != "")
                                        {
                                            int n = Convert.ToInt32(st);
                                            NoQuestResult += arrayfactors[n];
                                          
                                            arrayfactors[n] = 0;
                                           
                                           
                                        }
                                 
                                    }
                                    for (i = 0; i < arrayfactors.Length; ++i)
                                    {
                                        if (arrayfactors[i]!=0)
                                        {
                                            arrayfactors[i] = arrayfactors[i] / (1 - NoQuestResult);
                                          
                                        }
                                        result[tablenum - 1] += arrayfactors[i] * array[i];
                                    }

                                }
                                // Если ни одного отета не дано "НЕТ ОТВЕТА"
                                else
                                {
                                    
                                    for (i = 0; i < arrayfactors.Length; ++i)
                                        result[tablenum - 1] += arrayfactors[i] * array[i];
                                }

                                b.Background = new SolidColorBrush(Color.FromRgb(91, 21, 67));
                                isFinished = true;
                                for (int p = 0; p < noquest.Length; p++)
                                    noquest[p] = false;
                            };
                        }
                    }
                    else
                    {
                        MessageBox.Show("Закончите, пожалуйста, блок.");
                    }
                };
            }
        }
        // ВЫВЕСТИ РЕЗУЛЬТАТЫ ввод курсора в бордер
        private void Finish_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            Finish.Background = new SolidColorBrush(Color.FromRgb(6, 55, 135));
            FinisTxt.Foreground = new SolidColorBrush(Color.FromRgb(198, 204, 211));
            FinisTxt.FontSize = 18;
            FinisTxt.FontStyle = FontStyles.Italic;
        }
        // ВЫВЕСТИ РЕЗУЛЬТАТЫ убираем курсор из бордера
        private void Finish_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            Finish.Background = new SolidColorBrush(Color.FromRgb(20, 100, 229));
            FinisTxt.Foreground = Brushes.White;
            FinisTxt.FontSize = 16;
            FinisTxt.FontStyle = FontStyles.Normal;
        }
        // Нажатеие на ВЫвести результаты
        private void Finish_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isChanged == false)
            {
                Questions.Children.Clear();

                // Вывод результатов теста

                RichTextBox rtb = new RichTextBox();
                rtb.FontFamily = new FontFamily("Arial");
                rtb.FontSize = 14;
                rtb.BorderThickness = new Thickness(0);

                rtb.AppendText("\n");
                double odin = Min(Min(result[0], result[1]), Min(result[3], result[12]));
                double dva = Min(Min(result[5], result[6]), Min(result[7], result[8]));
                double tri = Min(Min(result[4], result[2]), result[9]);
                double chetyre = Min(result[11], result[10]);
                rtb.AppendText("Организационные аспекты : "+ Min(Min(result[0], result[1]),Min(result[3], result[12])));
                rtb.AppendText("\r\n\tm1 - "+result[0]+"\n\tm2 - " + result[1] +"\n\tm4 - " + result[3] +"\n\tm13 - " + result[12] +"\n");

                rtb.AppendText("Инфраструктура организации : "+ Min(Min(result[5], result[6]), Min(result[7], result[8])));
                rtb.AppendText("\r\n\tm6 - " + result[5] + "\n\tm7 - " + result[6] + "\n\tm8 - " + result[7] + "\n\tm9 - " + result[8] + "\n");

                rtb.AppendText("Управление доступом : "+ Min(Min(result[4], result[2]), result[9]));
                rtb.AppendText("\r\n\tm5 - " + result[4] + "\n\tm3 - " + result[2] + "\n\tm10 - " + result[9] + "\n");

                rtb.AppendText("Менеджмент инцидет : " + Min(result[11], result[10]));
                rtb.AppendText("\r\n\tm11 - " + result[10] + "\n\tm12 - " + result[11] + "\n");

                string mark = "Низкий";
                if (Min(Min(odin, dva), Min(tri, chetyre)) > 0.30 && Min(Min(odin, dva), Min(tri, chetyre)) <= 0.50) mark = "Ниже среднего";
                else if (Min(Min(odin, dva), Min(tri, chetyre)) > 0.50 && Min(Min(odin, dva), Min(tri, chetyre)) <= 0.85) mark = "Средний";
                else if (Min(Min(odin, dva), Min(tri, chetyre)) > 0.85 && Min(Min(odin, dva), Min(tri, chetyre)) <= 1) mark = "Высокий";
                rtb.AppendText("Итоговая оценка : " +Min(Min(odin,dva), Min(tri,chetyre))+"\n");
                rtb.AppendText("Уровень ИБ предприятия : " + mark);
                Questions.Children.Add(rtb);


            }
            else
            {
                MessageBox.Show("Закончите, пожалуйста, блок.");
            }
        }
        double Min(double a, double b)
        {
           
            if (a < b ) return a;
             else return b;
        }
    }
}
