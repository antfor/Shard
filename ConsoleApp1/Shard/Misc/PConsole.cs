using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shard.Misc
{
    enum CMD { 
        PRINT,
        CLEAR
    }
    class Command {
        public readonly CMD command;

        public Command(CMD cmd) {
            command = cmd;
        }
    }
    class Command<T>:Command {
        
        public readonly T value;

        public Command(CMD cmd, T val):base(cmd) {
            value = val;
        }
    }

    class CommandFactory {
        public static Command Clear() {
            return new Command(CMD.CLEAR);
        }

        public static Command<string> Print(string line) {
            return new Command<string>(CMD.PRINT, line);
        }
    }
   
    public class PConsole: IThread

    {
        private static ConcurrentQueue<Command> fifo = new ConcurrentQueue<Command> { };
        private static PConsole me;
        private static bool runnning = true;
        private static ThreadManager tm;
        public  static readonly string threadID = "console";
        private static readonly string semID = "sem_console";


        public static void init()
        {
            Console.WriteLine("consoleP");
            tm = ThreadManager.getInstance();
            PConsole con = new PConsole();
            tm.addThread(threadID, con);
            tm.runThread(threadID);

            tm.addSem(semID);
            me = con;
        }

        private PConsole() { }

        public static void WriteLine(string line) {
            Write("\n" + line);
        }
        public static void Write(string line) {
            Command cmd = CommandFactory.Print(line);
            fifo.Enqueue(cmd);
            tm.release(semID);
        }
        public static void Clear() {
            Command cmd = CommandFactory.Clear();
            fifo.Enqueue(cmd);
            tm.release(semID);
        }

        public void runMethod()
        {
            Command command;

            while (runnning) {
                
                tm.waitOne(semID);

                if (fifo.TryDequeue(out command))
                {

                    switch (command.command)
                    {
                        case CMD.CLEAR: Console.Clear(); break;
                        case CMD.PRINT: Console.Write(((Command<string>)command).value); break;

                    }
                }

            }
        }
    }
}
