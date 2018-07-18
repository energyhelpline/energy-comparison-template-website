using System;
using System.Collections.Generic;
using BareboneUi.Common;

namespace BareboneUi.Pages
{
    public class Question
    {
        private readonly Item _item;

        public Question(Item item) => _item = item;

        public void SetAnswer(string value) => _item.Data = value;
        public void SetAnswer(DateTime value) => _item.Data = value.ToString("yyyy-MM-ddTHH:mm:ss");
        public void SetAnswer(object value) => _item.Data = value?.ToString();

        public IEnumerable<KeyPair> DropDownValues => _item.AcceptableValues;

        public static explicit operator Item(Question question) => question._item;

        public static implicit operator string(Question question) => question.ToString();
        public override string ToString()
        {
            return _item?.Data;
        }
    }
}