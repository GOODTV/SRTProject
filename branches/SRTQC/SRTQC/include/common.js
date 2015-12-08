//左邊 menu 隱藏顯示控制
function InitMenu()
{
    var Switch_Status = parent.document.getElementsByTagName('frameset')[1].cols;
    if (Switch_Status == "162,*") {
        $('#menu_show').addClass('hidden');
    } else if (Switch_Status == "0,*") {
        $('#menu_hide').addClass('hidden');
    }

    $('#menu_hide').click(function () {
        $('#menu_show').removeClass('hidden');
        $('#menu_hide').addClass('hidden');
    });
    $('#menu_show').click(function () {
        $('#menu_hide').removeClass('hidden');
        $('#menu_show').addClass('hidden');
    });

    $.swapImage(".swapImage");
    $.swapImage(".swapImageClick", true, true, "click");
    $.swapImage(".swapImageDoubleClick", true, true, "dblclick");
    $.swapImage(".swapImageSingleClick", true, false, "click");
    $.swapImage(".swapImageDisjoint");
}

function SwitchMenu() {
    var Switch_Status = parent.document.getElementsByTagName('frameset')[1].cols;
    if (Switch_Status == "162,*") {
        parent.document.getElementsByTagName('frameset')[1].cols = '0,*';
    } else if (Switch_Status == "0,*") {
        parent.document.getElementsByTagName('frameset')[1].cols = '162,*';
    }
}
//----------------------------------------------------------------------------
//檢查日期
function CheckDateFormat(ControlID, FieldDesc)
{
    var alertStr = CheckDate(ControlID, FieldDesc, false);

    if (alertStr != "") {
        document.getElementById(ControlID).value = "";
        alert(alertStr);
    }
}
//----------------------------------------------------------------------------
//檢測中英文夾雜字串實際長度
function CheckLen(ControlID, Length, FieldDesc, blnRequest) {
    var control = GetControl(ControlID);
    var value = GetControlValue(ControlID);
    var strAlert = "";
    if (blnRequest == true) {
        strAlert = CheckControl("Request", ControlID, FieldDesc);
        if (strAlert != "")
            return strAlert;
    }
    else if (value == "") return "";


    if (blen(value) > Length) {
        return FieldDesc + " 欄位中文字不可超過" + (Math.floor(Length / 2)) + "個字或英文字不可超過" + Length + "個字!\r\n";
    }
    return "";
}
//----------------------------------------------------------------------------
//檢測中英文夾雜字串實際長度
function blen(str) {
    //參考網址
    //http://www.360doc.com/content/081222/15/16915_2177384.html
    var len = str.match(/[^ -~]/g) == null ? str.length : str.length + str.match(/[^ -~]/g).length;
    return len;
}
//----------------------------------------------------------------------------
//判斷是否為日期格式
function CheckDate(ControlID, FieldDesc, blnRequest) {
    var control = GetControl(ControlID);
    var value = GetControlValue(ControlID);
    if (blnRequest == true)
    {
        strAlert = CheckControl("Request", ControlID, FieldDesc);
        if (strAlert != "")
            return strAlert;
    }
    else if (value == "") return "";
    //規則 yyyy/mm/dd
    var obj, strValue, flag = false;
    strValue = GetControlValue(ControlID);
    if (strValue == "")
        return "";
    var objRegExp = /^\d{4}\/\d{1,2}\/\d{1,2}$/;

    //check to see if in correct format
    if (!objRegExp.test(strValue))
    {
        return FieldDesc + " 欄位必須為西元日期格式 ! (yyyy/mm/dd)\r\n";
        //doesn't match pattern, bad date
    }
    else
    {
        var strSeparator = '/';
        var arrayDate = strValue.split(strSeparator);
        //create a lookup for months not equal to Feb.
        var arrayLookup = { '01': 31, '03': 31,
            '04': 30, '05': 31,
            '06': 30, '07': 31,
            '08': 31, '09': 30,
            '1': 31, '3': 31,
            '4': 30, '5': 31,
            '6': 30, '7': 31,
            '8': 31, '9': 30,
            '10': 31, '11': 30, '12': 31
        }
        var intDay = parseInt(arrayDate[2], 10);

        //check if month value and day value agree
        if (arrayLookup[arrayDate[1]] != null)
        {
            if (intDay <= arrayLookup[arrayDate[1]] && intDay != 0)
                flag = true; //found in lookup table, good date
        }

        //check for February (bugfix 20050322)
        //bugfix  for parseInt kevin
        //bugfix  biss year  O.Jp Voutat
        var intMonth = parseInt(arrayDate[1], 10);
        if (intMonth == 2)
        {
            var intYear = parseInt(arrayDate[0]);
            if (intDay > 0 && intDay < 29)
            {
                flag = true;
            }
            else if (intDay == 29)
            {
                if ((intYear % 4 == 0) && (intYear % 100 != 0) ||
                    (intYear % 400 == 0))
                {
                    // year div by 4 and ((not div by 100) or div by 400) ->ok
                    flag = true;
                }
            }
        }
    }

    if (flag == false) {
        return FieldDesc + " 欄位日期資料有誤!\r\n";
    }
    return "";
}
//----------------------------------------------------------------------------
//取得控制項
function GetControlValue(ControlID)
{
    var control;
    var value;
    control = GetControl(ControlID);
    if (control.tagName.toLowerCase() == "select")
    {
        if (control.options.length == 0)
            value = "";
        else
            value = control.options[control.selectedIndex].value;
    }
    else
        value = control.value;
    return value
}
//----------------------------------------------------------------------------
//取得控制項的值
function GetControl(ControlID)
{
    return document.getElementById(ControlID);
}
//----------------------------------------------------------------------------
function CheckDateFormat(Control, FieldDesc)
{
    var alertStr = CheckDate(Control.id, FieldDesc, false);
    if (alertStr != "")
    {
        document.getElementById(Control.id).value = "";
        alert(alertStr);
    }
}
//----------------------------------------------------------------------------
function SetHiddenValue(form, HiddenValue, array) {
    $(form).submit(function () {
        var val = '';
        val = 'hiddenValueHeader|';
        $.each(array, function (index, value) {
            if (value.type == 'text') {
                val += value.id + '|' + value.type + '|' + $('#' + value.id).getValue() + "|";
            }
            else if (value.type == 'checkbox') {
                val += value.id + '|' + value.type + '|' + $('input:checkbox[name=' + value.id + ']').getValue() + "|";
            }
            else if (value.type == 'radio') {
                val += value.id + '|' + value.type + '|' + $('input:radio[name=' + value.id + ']').getValue() + "|";
            }
            //alert(value);
        })

        //$('#HFD_Value').val(val);
        $('#' + HiddenValue).val(val);
    })
}
//----------------------------------------------------------------------------
function GetHiddenValue(HiddenValue) {
    var hiddenValueArray = $('#' + HiddenValue).val().split('|');
    var Len = hiddenValueArray.length;
    if (Len > 0) {
        if (hiddenValueArray[0] == 'hiddenValueHeader') {
            for (var i = 1; i <= Len - 2; i += 3) {
                if (hiddenValueArray[i + 1] == 'text') {
                    $('#' + hiddenValueArray[i]).setValue(hiddenValueArray[i + 2]);
                }
                if (hiddenValueArray[i + 1] == 'checkbox') {
                    $('input:checkbox[name=' + hiddenValueArray[i] + ']').setValue(hiddenValueArray[i + 2]);
                }
                else if (hiddenValueArray[i + 1] == 'radio') {
                    $('input:radio[name=' + hiddenValueArray[i] + ']').setValue(hiddenValueArray[i + 2]);
                }
            }
        }
    }
}
//----------------------------------------------------------------------------
function initCalendar(array) {
    $.each(array, function (index, value) {
        Calendar.setup({
            inputField: "txt" + value,   // id of the input field
            button: "img" + value        // 與觸發動作的物件ID相同
        });
    })
}
//----------------------------------------------------------------------------
function String2Number(data) {
    if (data == '')
        return 0;
    try {
        var Num = parseInt(data, 10);
        if (isNaN(Num)) {
            return 0;
        }
        return Num;
    }
    catch (Error) {
        return 0;
    }
}
//----------------------------------------------------------------------------

function checkID(idStr) {
    // 依照字母的編號排列，存入陣列備用。
    var letters = new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'W', 'Z', 'I', 'O');
    // 儲存各個乘數
    var multiply = new Array(1, 9, 8, 7, 6, 5, 4, 3, 2, 1);
    var nums = new Array(2);
    var firstChar;
    var firstNum;
    var lastNum;
    var total = 0;

    // 撰寫「正規表達式」。第一個字為英文字母，
    // 第二個字為1或2，後面跟著8個數字，不分大小寫。
    var regExpID = /^[a-z](1|2)\d{8}$/i;

    // 使用「正規表達式」檢驗格式
    if (idStr.search(regExpID) == -1) {
        // 基本格式錯誤
        alert("身份證號碼錯誤！");
        return false;
    }
    else {
        // 取出第一個字元和最後一個數字。
        firstChar = idStr.charAt(0).toUpperCase();
        lastNum = idStr.charAt(9);
    }

    // 找出第一個字母對應的數字，並轉換成兩位數數字。
    for (var i = 0; i < 26; i++) {
        if (firstChar == letters[i]) {
            firstNum = i + 10;
            nums[0] = Math.floor(firstNum / 10);
            nums[1] = firstNum - (nums[0] * 10);
            break;
        }
    }

    // 執行加總計算
    for (var i = 0; i < multiply.length; i++) {
        if (i < 2) {
            total += nums[i] * multiply[i];
        }
        else {
            total += parseInt(idStr.charAt(i - 1)) * multiply[i];
        }
    }

    // 和最後一個數字比對
    if ((10 - (total % 10)) != lastNum) {
        alert("身份證號碼錯誤！");
        return false;
    }
    return true;
}

//----------------------------------------------------------------------------