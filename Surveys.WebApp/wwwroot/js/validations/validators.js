$.validator.addMethod("requiredchoices", function (value, element, params) {
    if (+params === 0) return true;
    return +$(element.parentElement).find(":checked").length >= +params;
});

$.validator.unobtrusive.adapters.add("requiredchoices", ["count"], function (options) {
    options.rules["requiredchoices"] = options.params.count;
    options.messages["requiredchoices"] = options.message;
});

$.validator.addMethod("allowedchoices", function (value, element, params) {
    if (+params === 0) return true;
    return +$(element.parentElement).find(":checked").length <= +params;
});

$.validator.unobtrusive.adapters.add("allowedchoices", ["count"], function (options) {
    options.rules["allowedchoices"] = options.params.count;
    options.messages["allowedchoices"] = options.message;
});
