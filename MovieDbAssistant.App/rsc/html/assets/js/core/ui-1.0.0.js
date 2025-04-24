/**
 * UI
 * ------------------
 * dependencies:
 *      util-1.0.0
 *      template-1.0.0
 *      ui-layout-1.0.0
 */

/**
 * ui engine
 * @class
*/
class UI {

    /**
     * @type {bool} window is windowed
     */
    isWindowed = false

    /**
     * @type {bool} window is maximized
     */
    isMaximized = false

    /**
     * @type {bool} window is minimized
     */
    isMinimized = false

    constructor() {
        window.ui = this

        if (inDesktopMode()) {
            const st = window.sessionStorage
            if (!st.getItem('app')) {
                console.log("add session storage");
                this.#storeAppProps(
                    app.isWindowed,
                    app.isMinimized,
                    app.isMaximized)
            } else {
                console.log("get session storage");
            }
            var pr = this.#getAppProps()
            this.isWindowed = pr.isWindowed
            this.isMinimized = pr.isMinimized
            this.isMaximized = pr.isMaximized
        }
    }

    #getAppProps() {
        const st = window.sessionStorage
        const str = st.getItem('app')
        const pr = JSON.parse(str)
        return pr
    }

    #storeAppProps(isWindowed, isMinimized, isMaximized) {
        console.log("update session storage");
        const st = window.sessionStorage
        const pr = {
            'isWindowed': isWindowed,
            'isMinimized': isMinimized,
            'isMaximized': isMaximized
        }
        st.setItem('app', JSON.stringify(pr))
    }

    /**
     * init after create
     */
    init() {
        // apply zoom scale
        this.#applyZoomScale()

        // add front props
        this.#setupVariables()
    }

    /**
     * setup after body tag loaded
     */
    beforeLoadSetup() {
        
        // apply window state css classes
        this.#applyWindowStateCssClasses()
    }

    /**
     * setup (on document ready)
     */
    setup() {

        // activate dialogs 'closer' buttons
        this.#setupClosersButtons()

        // activate draggables (TODO: drag.js)
        this.#setupDraggables()

        // setup zoom for windowed mode
        this.#enableAutoZoom()
    }

    /**
     * close current window
     * @param {HTMLElement} e
     */
    closeWindow(e) {

    }

    /**
     * mawimize current window
     * @param {HTMLElement} e
     */
    mawimizeWindow(e) {

    }

    /**
     * minimize current window
     * @param {HTMLElement} e
     */
    minimizeWindow(e) {

    }

    /**
     * restore current window
     * @param {HTMLElement} e
     */
    restoreWindow(e) {

    }

    /**
     * open a dialog
     * @param {HTMLElement} this caller
     * @param {string} tplEId id of the dialog tpl instance */
    openDialog(e, tplEId) {
        var $tpl = $(Query_Prefix_Id + tplEId)
        $tpl.fadeIn(Dialog_FadeIn_Time)
    }

    /**
     * close a dialog
     * @param {HTMLElement} this caller
     * @param {string} tplEId id of the dialog tpl instance */
    closeDialog(e, tplEId) {
        var $tpl = $(Query_Prefix_Id + tplEId)
        $tpl.fadeOut(Dialog_FadeOut_Time)
    }

    /* #region ----- drag ----- */

    activateDrag(elem, container) {
        var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;

        elem.onmousedown = dragMouseDown;

        const $container = $(container)
        const $e = $(elem)

        function dragMouseDown(ev) {
            //e = e || window.event;
            if (!ev) return
            ev.preventDefault();

            const e = ev.target

            // get the mouse cursor position at startup:
            pos3 = ev.clientX;
            pos4 = ev.clientY;
            $container.on(Event_MouseUp, () => {
                closeDragElement(e);
            })

            // call a function whenever the cursor moves:
            $container.on(Event_MouseMove, ev => {
                elementDrag(ev)
            })
            $e.addClass(Class_Dragging)
        }

        function elementDrag(e) {
            e.preventDefault();
            // calculate the new cursor position:
            pos1 = pos3 - e.clientX;
            pos2 = pos4 - e.clientY;
            pos3 = e.clientX;
            pos4 = e.clientY;
            // set the element's new position:
            elem.style.top = (elem.offsetTop - pos2) + Unit_Px;
            elem.style.left = (elem.offsetLeft - pos1) + Unit_Px;
        }

        function closeDragElement(e) {
            // stop moving when mouse button is released:
            $container.off(Event_MouseUp)
            $container.off(Event_MouseMove)
            $e.removeClass(Class_Dragging)
        }
    }

    /* #endregion */

    /**
     * setup variables
     */
    #setupVariables() {
        _tpl().setVar(Var_InDesktopMode, inDesktopMode())
    }

    /**
     * setup draggables
     */
    #setupDraggables() {
        var t = this
        $(Query_Prefix_Class + Class_Draggable).each((i, e) => {
            t.activateDrag(e, $(Tag_Body)[0])
        })
    }

    /**
     * activate closers buttons
     */
    #setupClosersButtons() {
        var t = this
        $(Query_Prefix_Class + Class_Dialog_Button_Closer).each((i, e) => {
            const $e = $(e)
            const id = $e.attr(Data_Dialog_Id)
            $e.on(Event_Click, ev => {
                t.closeDialog(e, id)
            })
        })
    }

    /**
     * activate zoom for windowed mode
     */
    #enableAutoZoom() {
        $(window).on(Event_Resize, ev => {
            this.#applyZoomScale()
        });
    }

    /**
     * apply zoom scale
     */
    #applyZoomScale() {
        const z = this.getZoomScale()

        $(Tag_Html)
            .css(Attr_Zoom, z);
        if (z > 0)
            $(Query_Prefix_Class + Class_Page_Container_App_Region)
                .css(Attr_Zoom, 1 / z)
    }

    /**
     * get zoom scale of current window
     * @returns
     */
    getZoomScale() {
        var $w = $(window)
        const winWidth = $w.width()
        const refWidth = 1920
        const z = winWidth / refWidth
        return z
    }

    /**
     * apply css classes that indicates ui states
     * @plateform electron
     */
    #applyWindowStateCssClasses() {
        const $body = $(Tag_Body)
        const isWindowed = this.isWindowed
        const isFullscreen = !isWindowed
        const isMaximized = this.isMaximized
        const isMinimized = this.isMinimized

        if (!isWindowed) $body.removeClass(Class_Ui_Windowed)
        else $body.addClass(Class_Ui_Windowed)
        if (!isFullscreen) $body.removeClass(Class_Ui_Fullscreen)
        else $body.addClass(Class_Ui_Fullscreen)
        if (!isMaximized) $body.removeClass(Class_Ui_Maximized)
        else $body.addClass(Class_Ui_Maximized)
        if (!isMinimized) $body.removeClass(Class_Ui_Minimized)
        else $body.addClass(Class_Ui_Minimized)
    }

    /**
     * handle a signal
     * @param {string} name signal name
     * @param {any} data data
     */
    signal(name, data) {

        var isWindowStateSignal = false

        switch (name) {
            case Signal_Window_State_Changed_Enter_FullScreen:
            case Signal_Window_State_Changed_Leave_FullScreen:
            case Signal_Window_State_Changed_Maximize:
            case Signal_Window_State_Changed_Unmaximize:
            case Signal_Window_State_Changed_Minimize:
            case Signal_Window_State_Changed_Restore:

                isWindowStateSignal = true
                var isWindowed = true
                if (name == Signal_Window_State_Changed_Enter_FullScreen) {
                    isWindowed = false
                    this.isMinimized = false
                    this.isMaximized = false
                }
                if (name == Signal_Window_State_Changed_Leave_FullScreen) {
                    isWindowed = true
                    this.isMinimized = false
                    this.isMaximized = false
                }
                this.isWindowed = isWindowed
                break
        }
        switch (name) {
            case Signal_Window_State_Changed_Leave_FullScreen:
            case Signal_Window_State_Changed_Unmaximize:
            case Signal_Window_State_Changed_Restore:
                this.isMaximized = false
                this.isMinimized = false
        }
        switch (name) {
            case Signal_Window_State_Changed_Maximize:
                this.isMaximized = true
        }
        switch (name) {
            case Signal_Window_State_Changed_Minimize:
                this.isMinimized = true
        }

        if (isWindowStateSignal) {
            this.#storeAppProps(this.isWindowed, this.isMinimized, this.isMaximized)
            this.#applyWindowStateCssClasses()
        }
    }

    // ---------- electron app ------------
    // class App ?

    canSend() {
        return app && app.send
    }

    /** 
     * minimize main window
     * @param {HTMLElement} e html element
     */
    minimize(e) {
        if (this.canSend())
            app.send(Command_Minize)
    }

    /**
     * maximize main window
     * @param {HTMLElement} e html element
     */
    maximize(e) {
        if (this.canSend())
            app.send(Command_Maximize)
    }

    /** 
     * fullscreen main window
     * @param {HTMLElement} e html element
     */
    fullscreen(e) {
        if (this.canSend())
            app.send(Command_FullScreen)
    }

    /** 
     * restore main window
     * @param {HTMLElement} e html element
     */
    restore(e) {
        if (this.canSend())
            app.send(Command_Restore)
    }

    /** 
     * close main window
     * @param {HTMLElement} e html element
     */
    close(e) {
        if (this.canSend())
            app.send(Command_Close)
    }
}
