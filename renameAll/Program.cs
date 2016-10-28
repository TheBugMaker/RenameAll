using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;  
namespace renameAll
{
    class Program
    {
        static void Main(string[] args)
        {   
             if (args.Length < 3)
            {
                Console.WriteLine("usage : renameAll [action] -o <string to replace> -n <new string>   [options]");
                Console.WriteLine();
                Console.WriteLine("list of actions : ");
                Console.WriteLine("\t -a \t\t rename files and directories");
                Console.WriteLine("\t -f \t\t rename files ");
                Console.WriteLine("\t -d \t\t rename directories ");
                Console.WriteLine();
                Console.WriteLine("specify a pattern for the target files");
                Console.WriteLine("\t -pf \t\t pattern for the affected files ");
                Console.WriteLine("\t -pd \t\t pattern for the affected directories ");
                Console.WriteLine();
                Console.WriteLine("specify the path of the target directory");
                Console.WriteLine("\t -p \t\t path of the directory ");
                
                 return ; 
            }

            var oldString = getArg("-o" , "",args) ; 
            var newString = getArg("-n" , oldString ,args) ; 
            var pattern = getArg("-pf" , "*",args) ;
            var patternd = getArg("-pd","*",args);
            var path = getArg("-p",".",args) ; 
            var target = "-a" ; 
          

            if(Array.IndexOf(args , "-d") > -1  ){
                target = "-d" ; 
            }
            if(Array.IndexOf(args , "-f") > -1 ){
                target = "-f" ; 
            }


            switch (target)
            {
                case "-a" :
                    renameDir(path, oldString, newString, patternd);
                    renameFiles(path, oldString, newString, pattern);
                    break; 
                case "-d":
                    renameDir(path, oldString, newString, patternd);
                    break;
                case "-f":
                    renameFiles(path, oldString, newString, pattern);
                    break; 

            }
            Console.WriteLine("DONE !");
            Console.WriteLine("Happy Hacking :)"); 

        }

     
        
        static void renameDir(string dir  , string old, string newName, string pattern)
        {
            var dirs = Directory.GetDirectories(dir, pattern , SearchOption.TopDirectoryOnly);
            foreach (var d in dirs)
            {
                var paths = d.Split(Path.PathSeparator);
                paths[paths.Length-1] = paths[paths.Length-1].Replace(old, newName);
                var newd = string.Join(""+Path.PathSeparator, paths);
                if (!newd.Equals(d))
                     Directory.Move(d, newd);
                renameDir(newd, old, newName, pattern); 
            }

        }

        static void renameFiles(string dir  ,string old , string newName , string pattern){
            var files = Directory.GetFiles(dir, pattern, SearchOption.AllDirectories);
            foreach (var f in files)
            {
                var paths = f.Split(Path.PathSeparator);
                paths[paths.Length - 1] = paths[paths.Length - 1].Replace(old, newName);
                var newf = string.Join("" + Path.PathSeparator, paths);
                if(!newf.Equals(f))
                    Directory.Move(f, newf);
            }
        }

        static string getArg(string cmd,string defaultVal  , string[] args)
        {            
            var p = Array.IndexOf(args,cmd);
           
            if (p > -1 && p < args.Length - 1)
            {
                var pattern = args[p + 1];
                if (pattern.IndexOf('-') != 0)
                {
                    return pattern; 
                }
            }
            return defaultVal; 
        }
    }
}
