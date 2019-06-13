using System;

[Flags]
public enum EDamageReceiver {
    Default = 1 << 0,
    Player = 1 << 8,
    Enemy = 1 << 9,
    Environment = 1 << 10,
}