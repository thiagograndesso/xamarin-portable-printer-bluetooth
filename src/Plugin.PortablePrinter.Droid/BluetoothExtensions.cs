using System;
using Android.Bluetooth;

namespace Plugin.PortablePrinter.Droid
{
    public static class BluetoothExtensions
    {
        public static void ThrowIfNotValid(this BluetoothAdapter adapter)
        {
            if (adapter == null)
                throw new Exception("No adapter has been found!");

            if (!adapter.IsEnabled)
                throw new Exception("Bluetooth is not enabled!");
        }
        
        public static void ThrowIfNotValid(this BluetoothDevice device)
        {
            if (device == null)
                throw new Exception("Could not connect to device. Please verify if bluetooth address is correct and try again!");
        }
    }
}