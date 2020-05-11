using System;
using System.Threading.Tasks;
using Android.Bluetooth;
using Java.IO;
using Java.Util;

namespace Plugin.PortablePrinter.Droid
{
    public class PortablePrinter
    {
        private BluetoothSocket _bluetoothSocket;
        private readonly string _macAddress;
        private readonly Formatter _textFormatter;
        private const string StandardSpp = "00001101-0000-1000-8000-00805F9B34FB";

        public PortablePrinter(string macAddress)
        {
            _macAddress = macAddress;
            _textFormatter = new Formatter();
        }

        public async Task Connect()
        {            
            var bluetoothAdapter = BluetoothAdapter.DefaultAdapter;                
            bluetoothAdapter.ThrowIfNotValid();
            
            var printer = bluetoothAdapter.GetRemoteDevice(_macAddress);
            printer.ThrowIfNotValid();
                
            _bluetoothSocket = printer.CreateInsecureRfcommSocketToServiceRecord(UUID.FromString(StandardSpp));

            await ConnectToBluetoothSocket();
        }       

        public async Task PrintText(string text, bool addLineFeeds = false)
        {
            if (!_bluetoothSocket.IsConnected) await ConnectToBluetoothSocket();
            
            await AddFormat();

            using var inReader = new BufferedReader(new InputStreamReader(_bluetoothSocket.InputStream));
            using var outReader = new BufferedWriter(new OutputStreamWriter(_bluetoothSocket.OutputStream));
            await outReader.WriteAsync(text);
            await outReader.NewLineAsync();

            if (addLineFeeds)
            {
                await outReader.NewLineAsync();
                await outReader.NewLineAsync();
                await outReader.NewLineAsync();
                await outReader.NewLineAsync();
                await outReader.NewLineAsync();
            }

            await outReader.FlushAsync();
                
            Java.Lang.Thread.Sleep(1000);
                
            inReader.Ready();
            await inReader.SkipAsync(0);
        }
        
        public void Close()
        {
            if (_bluetoothSocket.IsConnected) _bluetoothSocket.Close();
            _bluetoothSocket.Dispose();
        }
        
        private async Task ConnectToBluetoothSocket()
        {
            await _bluetoothSocket.ConnectAsync();
        }

        private async Task AddFormat()
        {
            var boldAndHeightFormat = _textFormatter
                .Bold()
                .Height()
                .Get();
            await _bluetoothSocket.OutputStream.WriteAsync(boldAndHeightFormat, 0, boldAndHeightFormat.Length);
        }       
    }
}
