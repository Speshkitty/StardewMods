using System;

namespace CustomiseChildBedroom
{
    [Flags]
    enum Bed
    {
        ALL = LEFT | RIGHT | CRIB,
        LEFT = 1 << 1,
        RIGHT = 1 << 2,
        CRIB = 1 << 3
    }
}
