function isTime(dtStr) {
    var dtTime = ":";

    var pos1 = dtStr.indexOf(dtTime);
    var strHour = dtStr.substring(0, pos1);
    var strMinutes = dtStr.substring(pos1 + 1);

    if (pos1 == -1) {
        alert("El formato del tiempo tiene que ser: hh:mm");
        return false;
    }
    if (isIntegerValue(strHour) == false) {
        alert("El formato de la hora no es válida");
        return false;
    }
    if (isIntegerValue(strMinutes) == false) {
        alert("El formato de los minutos no es válida");
        return false;
    }
    if (strHour < 0) {
        alert("Una hora menor de cero no está permitida");
        return false;
    }
    if (strHour >= 24) {
        alert("Una hora superior de 23 no está permitida");
        return false;
    }
    if (strMinutes < 0) {
        alert("Minutos menor de cero no estan permitidos");
        return false;
    }
    if (strMinutes >= 60) {
        alert("Minutos superior de 59 no estan permitidos");
        return false;
    }
    return true;
}

function isIntegerValue(s) {
    var i;
    for (i = 0; i < s.length; i++) {
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

