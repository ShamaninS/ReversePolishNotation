using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;

namespace PKF_BK_Studiya
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string _input = string.Empty; // переменная для хранения ввода
        private char _lastChar; // переменная содержит последний введенный символ

        #region Массивы, хранящие символы, для управления логикой добавления нового символа

        private readonly char[] _lineCanBeStartWith = new[] {'√', '(', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        private readonly char[] _charCanUseAfterMarks = new[] {'√', '(', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        private readonly char[] _isNumber = new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        private readonly char[] _charCanBeReplace = new[] {'^', '*', '/', '+', '-'};
        private readonly char[] _charCanUseAfterNumbers = new[] {'^' ,'*' ,'/' ,'+' ,'-', ',' ,')','%',
            '0','1','2','3','4', '5', '6', '7', '8', '9'};
        private readonly char[] _charCanUseAfterRightBrackets = new[] {')', '^', '*', '/', '+', '-'};

        #endregion
        
        public MainWindow()
        {
            InitializeComponent();
            MessageBox.Show("Для корректной работы калькулятора необходимо, чтобы каждая отдельная операция " +
                            "была отделена скобками, а между символами были пробелы. Например: ( 2 + ( ( 4 * 6 ) / 45 ) )." +
                            "\nДля того, чтобы посчитать сколько составляет а% от числа b - введите ( a % b )", "Справка");
        }

        #region События на нажатие кнопки (добавление,удаление ...)

        private void SetOne(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('1');
        }

        private void SetTwo(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('2');
        }

        private void SetThree(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('3');
        }

        private void SetFour(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('4');
        }

        private void SetFive(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('5');
        }

        private void SetSix(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('6');
        }

        private void SetSeven(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('7');
        }

        private void SetEight(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('8');
        }

        private void SetNine(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('9');
        }

        private void SetZero(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('0');
        }

        private void SetLeftBracket(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('(');
        }

        private void SetRightBracket(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput(')');
        }

        private void SetSquare(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('√');
        }

        private void SetMinus(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('-');
        }

        private void SetDivided(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('/');
        }

        private void SetMultiplication(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('*');
        }

        private void SetPlus(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('+');
        }

        private void SetElevation(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('^');
        }

        private void Comma(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput(',');
        }
        
        private void Percent(object sender, RoutedEventArgs e)
        {
            CheckAndSetInput('%');
        }

        private void ClearAll(object sender, RoutedEventArgs e)
        {
            _input = string.Empty;
            ShowResult(_input);
        }
        
        private void SetEquals(object sender, RoutedEventArgs e)
        {
            if (_input != String.Empty)
            {
                CompareCountOfBrackets();
            }
        }
        
        #endregion
        
        /// <summary>
        /// Метод управляет логикой добавления нового символа в InputBox
        /// </summary>
        /// <param name="value">Символ передаваемый с кнопку</param>
        private void CheckAndSetInput(char value)
        {
            if (string.IsNullOrEmpty(_input))
            {
                if (_lineCanBeStartWith.Contains(value)) _input += "( " + value;
            }
            else
            {
                _lastChar = _input[_input.Length - 1];

                switch (_lastChar) // 
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case ',':
                        if (_charCanUseAfterNumbers.Contains(value))
                        {
                            if (_isNumber.Contains(value) || value.Equals(','))
                            {
                                _input += value;
                            }
                            else _input += " " + value;
                        }

                        break;

                    default:
                        if (_charCanBeReplace.Contains(_lastChar) && _charCanBeReplace.Contains(value))
                        {
                            _input = _input.Remove(_input.Length - 1);
                            _input += value;
                        }
                        else
                        {
                            switch (_lastChar)
                            {
                                case ')':
                                    if (_charCanUseAfterRightBrackets.Contains(value)) _input += " " + value;
                                    break;
                                default:
                                {
                                    if (_charCanUseAfterMarks.Contains(value)) _input += " " + value;
                                    break;
                                }
                            }
                        } 
                        break;
                }
            }
            ShowResult(_input);
        }

       
        /// <summary>
        /// Метод выводит текст в InputBox
        /// </summary>
        /// <param name="output">Текст для вывода</param>
        private void ShowResult(string output)
        {
            InputBox.Text = output;
        }

        /// <summary>
        /// Метод проверяет количество открытых и закрытых скобок
        /// </summary>
        private void CompareCountOfBrackets()
        {
            int _leftBracketsCount = 0;
            int _rightBracketsCount = 0;
            foreach (var e in _input)
            {
                if (e.Equals('(')) _leftBracketsCount++;
                if (e.Equals(')')) _rightBracketsCount++;
            }

            if (_leftBracketsCount != _rightBracketsCount)
            {
                MessageBox.Show("Количество \"(\" и \")\" не совпадает!");
            }
            else
            {
                ConvertToReversePolishNotice();
            }
        }
        
        /// <summary>
        /// Метод преобразует исходную запись в обратную польскую нотацию и считает результат
        /// </summary>
        private void ConvertToReversePolishNotice()
        {
            try
            {
                string _rpn = string.Empty;

                List<double> _result  = new List<double>();
                Stack<string> _stack = new Stack<string>();

                foreach (var e in _input.Split(' '))
                {
                    try
                    {
                        double _number = Convert.ToDouble(e);
                        _result.Add(_number);
                        _rpn += e + " ";
                    }
                    catch
                    {
                        switch (e)
                        {
                            case "(":
                            case "+":
                            case "-":
                            case "/":
                            case "*":
                            case "^":
                            case "√":
                            case "%":
                               _stack.Push(e);
                                break;
                            case ")":
                                switch (_stack.Peek())
                                {
                                    case "+":
                                        double a = _result[_result.Count - 2];                    // этот участок кода повторяется
                                        double b = _result[_result.Count - 1];                    // я понимаю, что это плохая практика,
                                        _result.RemoveRange(_result.Count - 2, 2);    // но в данном случае участок кода слишком маленький
                                                                                                  // и я не вижу преймуществ убирать его в отдельный метод
                                        _result.Add(a+b);
                                        break;
                                    case "-":
                                        a = _result[_result.Count - 2];
                                        b = _result[_result.Count - 1];
                                        _result.RemoveRange(_result.Count - 2, 2);

                                        _result.Add(a-b);
                                        break;
                                    case "/":
                                        a = _result[_result.Count - 2];
                                        b = _result[_result.Count - 1];
                                        _result.RemoveRange(_result.Count - 2, 2);

                                        _result.Add(a/b);
                                        break;
                                    case "*":
                                        a = _result[_result.Count - 2];
                                        b = _result[_result.Count - 1];
                                        _result.RemoveRange(_result.Count - 2, 2);

                                        _result.Add(a*b);
                                        break;
                                    case "^":
                                        a = _result[_result.Count - 2];
                                        b = _result[_result.Count - 1];
                                        _result.RemoveRange(_result.Count - 2, 2);

                                        _result.Add(Math.Pow(a,b));
                                        break;
                                    case "√":
                                        b = _result[_result.Count - 1];
                                        _result.RemoveRange(_result.Count - 1, 1);

                                        _result.Add(Math.Sqrt(b));
                                        break;
                                    case "%":
                                        a = _result[_result.Count - 2];
                                        b = _result[_result.Count - 1];
                                        _result.RemoveRange(_result.Count - 2, 2);

                                        _result.Add(a/b*100);
                                        break;
                                }
                                _rpn += _stack.Pop() + " ";
                                _stack.Pop();
                                break;
                        }
                    }
                }
                _input = _rpn + " = " + _result[0];
                ShowResult(_input);
            }
            catch
            {
                MessageBox.Show("Проверьте правильность ввода данных");
            }
            
        }
    }
}