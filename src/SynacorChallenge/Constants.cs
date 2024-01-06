namespace SynacorChallenge;

public static class Constants
{
    public const ushort HaltInstruction = 0x00;
    public const ushort SetInstruction = 0x01;
    public const ushort PushInstruction = 0x02;
    public const ushort PopInstruction = 0x03;
    public const ushort EqInstruction = 0x04;
    public const ushort GtInstruction = 0x05;
    public const ushort JmpInstruction = 0x06;
    public const ushort JtInstruction = 0x07;
    public const ushort JfInstruction = 0x08;
    public const ushort AddInstruction = 0x09;
    public const ushort MultInstruction = 0x0A;
    public const ushort ModInstruction = 0x0B;
    public const ushort AndInstruction = 0x0C;
    public const ushort OrInstruction = 0x0D;
    public const ushort NotInstruction = 0x0E;
    public const ushort RmemInstruction = 0x0F;
    public const ushort WmemInstruction = 0x10;
    public const ushort CallInstruction = 0x11;
    public const ushort RetInstruction = 0x12;
    public const ushort OutInstruction = 0x13;
    public const ushort InInstruction = 0x14;
    public const ushort NoopInstruction = 0x15;
}