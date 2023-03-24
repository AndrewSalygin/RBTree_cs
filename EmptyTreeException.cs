using System;

namespace RBTree {
    class EmptyTreeException : Exception {
        public EmptyTreeException(string msg)
                : base(msg) { }
    }
}
