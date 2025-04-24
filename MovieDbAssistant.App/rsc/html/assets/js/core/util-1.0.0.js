/*#region ----- consts ----- */

/*#region global */

const Dot = '.'
const Space = ' '
const Slash = '/'
const Dash = '-'

const Text_Null = 'null'
const Text_Empty = ''

const Type_Name_Object = 'object'
const Type_Name_Array = 'Array'

const Path_Current = './'

const Event_Resize = 'resize'
const Event_Load = 'load'
const Event_Error = 'error'
const Event_Click = 'click'
const Event_MouseUp = 'mouseup'
const Event_MouseMove = 'mousemove'

const Signal_Window_DidNavigate = 'Signal_Window_DidNavigate'
const Signal_Window_State_Changed_Enter_FullScreen = 'Signal_Window_State_Changed_Enter_FullScreen'
const Signal_Window_State_Changed_Leave_FullScreen = 'Signal_Window_State_Changed_Leave_FullScreen'
const Signal_Window_State_Changed_Minimize = 'Signal_Window_State_Changed_Minimize'
const Signal_Window_State_Changed_Restore = 'Signal_Window_State_Changed_Restore'
const Signal_Window_State_Changed_Maximize = 'Signal_Window_State_Changed_Maximize'
const Signal_Window_State_Changed_Unmaximize = 'Signal_Window_State_Changed_Unmaximize'

/*#endregion*/

/*#region kernel */

const Var_System = 'sys';
const Var_Clock = Var_System + '.now';
const Var_InDesktopMode = Var_System + '.inDesktopMode';

/*#endregion*/

/** egion ui */

const HRef_Id_Separator = '#'

const Dialog_FadeIn_Time = 100
const Dialog_FadeOut_Time = 200
const List_FadeIn_Time = 300
const Details_FadeIn_Time = 50

const Data_HRef = 'data-href'
const Data_Target = 'data-target'
const Data_Dialog_Id = 'data-dialog-id'

const Attr_Src = 'src'
const Attr_Id = 'id'
const Attr_Class = 'class'
const Attr_Zoom = 'zoom'

const Class_Ui_Windowed = 'window-windowed'
const Class_Ui_Fullscreen = 'window-fullscreen'
const Class_Ui_Maximized = 'window-maximized'
const Class_Ui_Minimized = 'window-minimized'

const Class_Page_Container_App_Region = 'page-app-region-container'
const Class_Hidden = 'hidden'
const Class_Clock_With = 'with-clock'
const Class_Date_With = 'with-date'
const Class_Alternate_Pic_List_Enabled = 'alternate-pic-list-enabled'
const Class_Movie_Page_Detail = 'movie-page-detail'
const Class_Movie_Page_List = 'movie-page-list'
const Class_Movie_List = 'movie-list'
const Class_Movie_List_Item = 'movie-list-item'
const Class_Alternate_Pic_List = 'alternate-pic-list'
const Class_Dialog_Hidden = "dialog-hidden"
const Class_Dialog_Visible = "dialog-visible"
const Class_Dialog_Button_Closer = 'dialog-button-closer'
const Class_Draggable = 'draggable'
const Class_Dragging = 'dragging'

const Id_Item_Model = 'ItemModel';

const Tag_Body = 'body';
const Tag_Html = 'html';

const Query_Prefix_Class = '.'
const Query_Prefix_Id = '#'
const Query_Select_Prefix = '['
const Query_Select_Postfix = ']'
const Query_Contains_Class_Prefix = "[class*='"
const Query_Selector_Postfix = "']"
const Query_Equals_Id_Prefix = "[id='"

const Unit_Px = 'px'

const Undefined = 'undefined';

/*#endregion*/

/* #region electron */

const Command_FullScreen = 'Command_FullScreen'
const Command_Minize = 'Command_Minimize'
const Command_Maximize = 'Command_Maximize'
const Command_Restore = 'Command_Restore'
const Command_Close = 'Command_Close'

/* #endregion */

/*#endregion ----- */

/*#region ----- kernel ----- */

/**
 * gets the template object 
 * @returns {Template} object template
 */
function _tpl() {
    return window.tpl
}

/**
 * gets the layout object
 * @returns {UILayout} object layout
 */
function _layout() {
    return window.layout
}

/**
 * gets the ui object
 * @returns {UI} ui object
 */
function _ui() {
    return window.ui
}

/**
 * indicates if currently running in electron app
 * @returns
 */
function inDesktopMode() {
    return (typeof IN_DESKTOP_ENV) != Undefined;
}

/**
 * handle a signal
 * @param {string} name signal name
 * @param {json?} data json data
 */
function signal(name, data) {
    //var data = JSON.parse(dataStr)
    var str = JSON.stringify(data)
    console.log("⭐⚡SIGNAL " + name + " 📚 " + str)
    // publish
    _ui().signal(name, data)
}

/**
 * send a signal
 * @param {string} name signal name
 * @param {json?} data json data
 */
function send(name, data) {
    var str = JSON.stringify(data)
    console.log("⚡ SIGNAL ⚡ " + name + " 📚 " + str)
    // electron
    if (app && app.signal)
        app.send(name, data)
}

/*#endregion ----- */

/*#region strings */

/**
 * first letter in lower case
 * @param {string} txt text
 * @returns text
 */
function firstLower(txt) {
    return txt.charAt(0).toLowerCase() + txt.slice(1);
}

/*#endregion */