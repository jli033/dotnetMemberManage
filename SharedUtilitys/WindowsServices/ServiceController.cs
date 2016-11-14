using System;
using System.ServiceProcess;

namespace SharedUtilitys.WindowsServices
{
	public class ServiceController
	{
		private readonly string _serviceName;
		private System.ServiceProcess.ServiceController _service;
		private readonly bool _isServiceNotFound;

		public bool Status { get; set; }

        public static ServiceController GetInstance(string serviceName)
        {
            return new ServiceController(serviceName);
        }

		public ServiceController(string serviceName)
		{
			_serviceName = serviceName;
			Status = false;

			try
			{
				_service = new System.ServiceProcess.ServiceController(_serviceName);

                if (_service.Status == ServiceControllerStatus.Running)
				{
					Status = true;
				}
			}
			catch (Exception)
			{
				_isServiceNotFound = true;
			}
		}

		public bool Start()
		{
			if (_isServiceNotFound)
			{
				return false;
			}

			_service = new System.ServiceProcess.ServiceController(_serviceName);

			if (_service.Status == ServiceControllerStatus.Stopped)
			{
                _service.Start();
                _service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 1, 0));
                Status = true;
			}

			return true;
		}

		public void Stop()
		{
			Status = false;

			if (_isServiceNotFound)
			{
				return;
			}

			_service = new System.ServiceProcess.ServiceController(_serviceName);

			if (_service.Status == ServiceControllerStatus.Running)
			{
				_service.Stop();

                for (int i = 0; i < 5; i++)
                {
                    if (_service.Status == ServiceControllerStatus.Stopped)
                    {
                        break;
                    }

                    System.Threading.Thread.Sleep(500);
                }

			    Status = false;
			}
		}
	}
}
