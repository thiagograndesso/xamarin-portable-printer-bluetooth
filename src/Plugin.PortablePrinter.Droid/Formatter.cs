namespace Plugin.PortablePrinter.Droid
{
    public class Formatter
    {        
        private readonly byte[] _format;

        public Formatter()
        {            
            _format = new byte[] { 27, 33, 0 };
        }

        public byte[] Get()
        {
            return _format;
        }

        public Formatter Bold()
        {            
            _format[2] = (byte)(0x8 | _format[2]);
            return this;
        }

        public Formatter Small()
        {
            _format[2] = (byte)(0x1 | _format[2]);
            return this;
        }

        public Formatter Height()
        {
            _format[2] = (byte)(0x10 | _format[2]);
            return this;
        }

        public Formatter Width()
        {
            _format[2] = (byte)(0x20 | _format[2]);
            return this;
        }

        public Formatter Underlined()
        {
            _format[2] = (byte)(0x80 | _format[2]);
            return this;
        }
    }
}