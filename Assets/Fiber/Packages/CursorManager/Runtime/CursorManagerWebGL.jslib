mergeInto(LibraryManager.library, {

  // See list of cursors here: https://developer.mozilla.org/en-US/docs/Web/CSS/cursor
  SetBrowserCursor: function (cursor) {
    const cursorString = UTF8ToString(cursor);
    const unityContainer = document.querySelector("#unity-canvas");
    unityContainer.style.cursor = cursorString;
    console.log(`New cursor ${cursorString} set on`, unityContainer);
  },

});