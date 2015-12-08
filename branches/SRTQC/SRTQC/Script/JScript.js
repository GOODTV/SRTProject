
function chkData(objClientID) {
    testReg = /[^0-9]/;
    var Data = document.getElementById(objClientID).value;

    if (Data.match(testReg)) {
        alert('請輸入數字');
        document.getElementById(objClientID).value = '';
        return false;
    }
}

//Begin for EPG/CUE/EPG_query
//檢查日期的正確與範圍
function checkDateStartEnd(StartDate, StartDatefieldName, EndDate, EndDatefieldName) {

    var dt1 = StartDate;
    var dt2 = EndDate;

    if (dt1 == "") {
        alert(StartDatefieldName + "未輸入！");
        return false;
    }
    if (dt2 == "") {
        alert(EndDatefieldName + "未輸入！");
        return false;
    }

    if (!dateValidationCheck(dt1)) {
        alert(StartDatefieldName + "格式須為yyyymmdd");
        return false;
    }
    if (!dateValidationCheck(dt2)) {
        alert(EndDatefieldName + "格式須為yyyymmdd");
        return false;
    }

    var dt1 = new Date(dt1);
    var dt2 = new Date(dt2);

    if ((dt1 - dt2) > 0) {
        alert(StartDatefieldName + "至" + EndDatefieldName + "的範圍錯誤！");
        return false;
    }

    if ((dt2 - dt1) / (1000 * 60 * 60 * 24) >= 62) {
        alert(StartDatefieldName + "至" + EndDatefieldName + "的範圍太大！請選擇二個月內範圍。");
        return false;
    }
    else {
        return true;
    }
    return false;

}

//檢查日期的正確
function dateValidationCheck(str) {

    var re = new RegExp("^([0-9]{4})[./]{1}([0-9]{2})[./]{1}([0-9]{2})$");
    var strDataValue;
    var infoValidation = true;

    if ((strDataValue = re.exec(str)) != null) {
        var i = Date.parse(str);
        if (isNaN(i)) {
            infoValidation = false;
        }
        else {
            infoValidation = validDate(strDataValue[1], strDataValue[2], strDataValue[3]);
        }
    } else {
        infoValidation = false;
    }
    return infoValidation;
}

//額外檢查日期的正確(針對javascript，遇到2/30會自動轉換成3/2，讓日期不會錯誤)
function validDate(_year, _month, _day) {
    var Day = new Date(_year + '/' + _month + '/' + _day);
    var chk = new Date(Day.valueOf());
    return (chk.getFullYear() == _year && (chk.getMonth() + 1) == _month && chk.getDate() == _day);
}

//檢查播出時間和結束時間的正確與範圍
function chkCalendarTimeStartEnd(str1, strfieldName1, str2, strfieldName2) {

    if (str1 == "" && str2 != "") {
        alert(strfieldName1 + "未輸入！");
        return false;
    }
    /* 只填播出時間就列出之後的資料
    if (str1 != "" && str2 == "") {
        //alert(strfieldName2 + "未輸入！");
        //return false;
    }
    */
    if (str1 == "" && str2 == "") {
        return true;
    }

    if (str1 != "") {
        var chkCalendarTime_ok = chkCalendarTime(str1, strfieldName1);
        if (!chkCalendarTime_ok) return false;
    }
    if (str2 != "") {
        var chkCalendarTime_ok = chkCalendarTime(str2, strfieldName2);
        if (!chkCalendarTime_ok) return false;
    }
    if (str1 != "" && str2 == "") {
        return true;
    }else    if (str2 > str1) {
        return true;
    }
    else {
        alert(strfieldName1 + "至" + strfieldName2 + "的範圍錯誤！");
        return false;
    }

}

//檢查播出時間
function chkCalendarTime(str, strfieldName) {

    testReg = /[^0-9]/;
    var Data = str;

    if (Data.match(testReg)) {
        alert(strfieldName + '欄位請輸入數字');
        return false;
    }

    if (Data.length != 4) {
        alert(strfieldName + '欄位必須輸入4位數字的時分(hhmm)');
        return false;
    }

    var ck = parseInt(Data.substr(0, 2), 10);
    if (ck > 24) {
        alert(strfieldName + '欄位的小時數字不能超過24小時');
        return false;
    }

    var ck = parseInt(Data.substr(2, 2), 10);
    if (ck > 60) {
        alert(strfieldName + '欄位的分鐘數字不能超過60分鐘');
        return false;
    }
    return true;
}

//End for EPG/CUE/EPG_query

