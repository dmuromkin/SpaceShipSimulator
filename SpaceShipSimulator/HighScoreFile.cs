using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace SpaceShipSimulator
{
   public class HighScoreFile
    {
        private int _highscore;
        private string _scorePath;

        // получение последнего рекорда
        public int Read()
        {
            _scorePath= Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Highscore.txt");
            
            // если файл не существует - создаем файл и пишем туда 0
            if (!File.Exists(_scorePath))
            {
                using (FileStream fs = File.Create(_scorePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("0");
                    fs.Write(info, 0, info.Length);
                }
                return 0;
            }
            else
            { 
                using (StreamReader sr = new StreamReader(_scorePath))
                _highscore = Convert.ToInt32(sr.ReadToEnd());
            }
            return _highscore;
        }

        // запись нового рекорда
        public void Write(int highscore)
        {
            _scorePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Highscore.txt");
            _highscore = highscore;
            using (StreamWriter sw = new StreamWriter(_scorePath))
            {
                sw.WriteLine(_highscore.ToString());
            }
        }
    }
}
