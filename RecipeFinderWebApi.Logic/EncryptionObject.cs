﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Logic
{
    public class EncryptionObject
    {
        public string Text { get; set; }
        public string Salt { get; set; }
        public string Result { get; set; }
    }
}
