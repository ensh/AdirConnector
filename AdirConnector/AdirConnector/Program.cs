    using System;
    using AD.Common.Helpers;
    using AD.Common.DataStructures;

    namespace ADirConnector
    {
        class Program
        {
            static void Main(string[] args)
            {
                // настройка системы логирования
                LogFileManager.Instance.FolderPath = @"C:\TEMP\AdirConnector\Logs\";
                LogFileManager.Instance.LoggingLevel = EventsLoggingLevels.All;
                LogFileManager.Instance.SizeLimit = 32u << 20;

                RunProcessing(); // программа где-то здесь

                Console.ReadLine(); // ожидаем нажатия Enter, что приведет к дисконнекту
                Core.ADConnection.Instance.Disconnect();

                Console.ReadLine(); // ожидаем нажатия Enter, что приведет к выходу из программы
                LogFileManager.Instance.WriteAllStream(); // сбрасываем на диск все логи, из буферов
            }

            static void Instance_ConnectionChanging(Core.LogicConnectionStatus status)
            {
                Console.WriteLine(String.Format("Instance_ConnectionChanging({0})", status.ToString()));
            }

            static void Instance_OnConnectionStatusChanged(FrontEndType frontendType, ConnectionStatus status)
            {
                Console.WriteLine(String.Format("Instance_ConnectionChanged({0}, {1})", frontendType.ToString(), status.ToString()));
            }

            static void RunProcessing()
            {
                Core.ConnectionInfo.Instance.Load(ApplicationPaths.ConnectionInfoFilePath);

                Core.ADConnection.Instance.ConnectionChanging += Instance_ConnectionChanging;
                Core.ADConnection.Instance.OnFrontEndConnectionStatusChanged += Instance_OnConnectionStatusChanged;

                Console.Write("Login=>");
                var login = Console.ReadLine();

                Console.Write("Password=>");
                var password = Console.ReadLine();

                Core.ADConnection.Instance.Login(login, password);
            }
        }
    }
