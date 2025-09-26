mergeInto(LibraryManager.library, {
  BrowserConsoleLog: function (strPtr) {
    var msg = UTF8ToString(strPtr);
    console.log(msg); // или console.error(msg)
  }
});
