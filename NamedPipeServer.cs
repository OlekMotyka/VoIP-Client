using BSF_VoIP;
using System;
using System.IO.Pipes;

namespace BFS_VoIP
{
	public class NamedPipeServer : PipeStreamWrapperBase<NamedPipeServerStream>
	{
		public NamedPipeServer(string pipeName)
			: base(pipeName)
		{

		}

		~NamedPipeServer()
		{
			if (Pipe != null) Pipe.Dispose();
		}

		protected override bool AutoFlushPipeWriter
		{
			get { return true; }
		}

		protected override NamedPipeServerStream CreateStream()
		{
			try
			{
				return new NamedPipeServerStream(PipeName,
						   PipeDirection.InOut,
						   1,
						   PipeTransmissionMode.Message,
						   PipeOptions.Asynchronous,
						   BUFFER_SIZE,
						   BUFFER_SIZE);
			}
			catch { return null; }
		}

		protected override void ReadFromPipe(object state)
		{
			try
			{
				while (Pipe != null && m_stopRequested == false)
				{
					if (Pipe.IsConnected == false) Pipe.WaitForConnection();

					byte[] msg = ReadMessage(Pipe);

					ThrowOnReceivedMessage(msg);
				}
			}
			catch { }
		}
	}
}
