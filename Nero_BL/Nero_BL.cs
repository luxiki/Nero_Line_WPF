using System;
using System.Collections.Generic;
using System.IO;

namespace Nero_Line_WPF
{

    public class Nero_BL 
    {
        private string[] content;
        private string openFilePath;
        private string openFileName;
        private readonly string directory = "Line\\";
        public event EventHandler<EventMessage> SendMessage;

        public bool CheckFile(string filePath)
        {
            bool result = false;
            int errorLine = 0;
            try
            {
                if (!File.Exists(filePath))
                {
                    SendMessage(null, new EventMessage(@"Не возможно открыть фаил"));
                    return result;
                }

                content = File.ReadAllLines(filePath);

                foreach (string i in content)
                {
                    errorLine++;
                    int t = 0;
                    if (i != "")
                    {
                        for (int j = 0; j < i.Length; j++)
                        {
                            if ('\t' == i[j])
                            {
                                t++;
                            }
                        }

                        if (t != 4)
                        {
                            SendMessage(null, new EventMessage(@"Фаил повреждён в строке" + errorLine));
                            return result;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                SendMessage(null, new EventMessage(@"Не возможно открыть фаил" + ex.Message));
                return result;
            }
            string[] temp = filePath.Split('\\');
            openFileName = temp[temp.Length-1];
            openFilePath = filePath;
            result = true;
            SendMessage(null, new EventMessage(@"Фаил открыт. Выберите линию и схраните фаил!"));
            return result;
         }

        public bool SaveFile(string filePath)
        {
            int errorLine = 1;
            try
            {
                if (!File.Exists(directory + filePath + ".ini"))
                {
                SendMessage(null, new EventMessage(@"Фаил линии не найден!!!")); return false ; }
                Dictionary<string, int> map = new Dictionary<string, int>();
                string[] strLine = File.ReadAllLines(directory + filePath + ".ini");
                string[] strSplitLine;
                int itemp;
               
                for (int i = 1; i < strLine.Length; i++)
                {
                    errorLine++;
                    if (strLine[i] != "")
                    {
                        strSplitLine = strLine[i].Split(':');
                        if (strSplitLine.Length == 2)
                        {
                            map.Add(strSplitLine[0], int.Parse(strSplitLine[1]));
                        }
                        else
                        {
                            SendMessage(null, new EventMessage("Фаил линии повреждён в строке " + errorLine + " !!!"));
                            return false;
                        }
                }
                }

                
                for (int i = 0; i < content.Length; i++)
                {
                    if (content[i] != "")
                    {
                        strSplitLine = content[i].Split('\t');

                        if (map.ContainsKey(strSplitLine[1]))
                        {
                            itemp = map[strSplitLine[1]];
                            itemp += int.Parse(strSplitLine[4]);
                            if (itemp >= 180) { itemp -= 360; }
                            if (itemp <= -180) { itemp += 360; }
                            strSplitLine[4] = Convert.ToString(itemp);
                        }


                        if (strSplitLine[0][0] == 'R' || strSplitLine[0][0] == 'C' || strSplitLine[0][0] == 'L' || strSplitLine[0][0] == 'S')
                        {
                            itemp = int.Parse(strSplitLine[4]);
                            if (itemp > 90) { strSplitLine[4] = Convert.ToString(itemp - 180); }
                            if (itemp < -90) { strSplitLine[4] = Convert.ToString(itemp + 180); }
                        }


                        content[i] = strSplitLine[0] + '\t' + strSplitLine[1] + '\t' + strSplitLine[2] + '\t' + strSplitLine[3] + '\t' + strSplitLine[4];

                    }

                }

                string fls;
                if (Directory.Exists(strLine[0])) { fls = strLine[0] + "\\"  + openFileName; }
                else { fls = openFilePath + ".txt"; }

                File.WriteAllLines(fls, content);
                SendMessage(null, new EventMessage("Фаил сохранён в " + fls));
                return true;
            }
            catch (Exception ex)
            {
                SendMessage(null, new EventMessage("Ошибка в файле!!!" + ex.Message));
                return false;
            }

        }

        //public string AddSmd(string filePath, string addNameSmd , string addRotation)
        //{
        //    try
        //    {
        //        int r = Convert.ToInt32(addRotation);
        //        string[] strLine;
        //        string[] strSplitLine;

        //        if (File.Exists(filePath+".ini"))
        //        {
        //            strLine = File.ReadAllLines(filePath + ".ini");
        //        }
        //        else return "Фаил линии не найден";

        //        for (int i = 1; i < strLine.Length; i++)
        //        {
        //            strSplitLine = strLine[i].Split(':');
        //            if (addNameSmd==strSplitLine[0])
        //            {
        //                return "Такой компонент уже существует";
        //            }
        //        }
        //        string[] str = new string[1];
        //        str[0] = string.Format("{0}:{1}",addNameSmd,addRotation);
                
        //        File.AppendAllLines(filePath + ".ini" , str);

                
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Error " + ex.Message;
        //    }

            
        //    return "Компонент успешно добавлен!";
        //}

        public string[] GetLine()
        {
            string[] nameFiles = Directory.GetFiles("Line" , "*.ini");
            string[] result = new string[nameFiles.Length];
            
            for (int i = 0; i < nameFiles.Length; i++)
            {
                string[] temp1 = nameFiles[i].Split('\\');
                string[] temp2 = temp1[1].Split('.');
                result[i] = temp2[0];

            }
           
            return result;
        }


    }
}
