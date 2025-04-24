/**
 * UI Layout
 * ------------------
 * dependencies:
 *      template-1.0.0
 *      util-1.0.0
 */

//const Class_Movie_List = ''

/**
 * ui layout
 * @class
*/
class UILayout {

    constructor() {
        window.layout = this
    }

    /**
     * enable widgets
     */
    enableGlobalWidgets() {
        this.#enableClock()
        this.#enableDate()
    }

    /**
     * post setup : after tpl built
     */
    postSetup() {        
        this.#setAlternatePics()
        return this
    }

    /**
     * get var now
     * @returns
     */
    getVarNow() {
        if (!_tpl().getVar(Var_Clock))
            _tpl().setVar(Var_Clock,
                { date: null, clock: null })
        return _tpl().getVar(Var_Clock)
    }

    /**
     * enable date update
     */
    #enableDate() {
        this.#dateUpdate()
        setTimeout(() => this.#enableDate(), 1000 * 30)
    }

    /**
     * update the date
     */
    #dateUpdate() {
        const now = new Date()
        const day = now.getDay();
        const date = now.getDate();
        const month = now.getMonth();
        //const year = now.getFullYear();
        const str = `${dayNames[day].substring(0, 3)} ${date} ${monthNames[month].substring(0, 3)}`;

        var vnow = this.getVarNow()
        vnow.date = str

        $(Query_Prefix_Class + Class_Date_With).html(str)
    }

    /**
     * enable clock update
     */
    #enableClock() {
        this.#clockUpdate()
        setTimeout(() => this.#enableClock(), 1000 * 30)
    }

    /**
     * clock update
     */
    #clockUpdate() {
        this.clockUpdating = true
        const now = new Date()
        const hours = now.getHours().toString().padStart(2, '0');
        const minutes = now.getMinutes().toString().padStart(2, '0');
        const seconds = now.getSeconds().toString().padStart(2, '0');
        //const str = hours + " : " + minutes + " : " + seconds;
        const str = hours + " : " + minutes;

        var vnow = this.getVarNow()
        vnow.clock = str

        $(Query_Prefix_Class + Class_Clock_With).html(str)
        this.clockUpdating = false
    }

    #imgCount = 0

    /**
     * set alternate pics values
     */
    #setAlternatePics() {
        var $pics = $(
            Query_Prefix_Class + Class_Movie_Page_List
            + Space
            + Query_Prefix_Class + Class_Alternate_Pic_List)

        this.#imgCount = $pics.length
        var altUrl = props.tpl.props.listMoviePicNotAvailable;
        var altnfUrl = props.tpl.props.listMoviePicNotFound;
        this.#setupAlternatePic($pics, altUrl, altnfUrl)

        $pics = $(
            Query_Prefix_Class + Class_Movie_Page_Detail
            + Space
            + Query_Prefix_Class + Class_Alternate_Pic_List)

        this.#imgCount += $pics.length
        altUrl = props.tpl.props.detailMoviePicNotAvailable;
        altnfUrl = props.tpl.props.detailMoviePicNotFound;
        this.#setupAlternatePic($pics, altUrl, altnfUrl)
    }

    onPicLoaded = null
    #imgNum = 0

    /**
     * setup alternate pics
     * @param {any} $set items
     * @param {any} altUrl alternate url
     * @param {any} altnfUrl alternate url if src null or white spaces
     */
    #setupAlternatePic($set, altUrl, altnfUrl) {
        const t = this
        $set.each((i, e) => {
            var $e = $(e)

            $e.on(Event_Load, e => {
                t.#imgNum++
                const $item = $(e.target).closest('movie-list-item-container')
                //$item.fadeIn(List_Item_FadeIn_Time)
                //console.debug(t.#imgNum + ' / ' + t.#imgCount)
                if (t.#imgNum == t.#imgCount) {
                    // all pic loaded
                    if (t.onPicLoaded) t.onPicLoaded()
                }
            })

            $e.on(Event_Error, () => {
                const src = $e.attr(Attr_Src)
                const url = (!src || src == Text_Empty || src == Text_Null) ?
                    altUrl : altnfUrl
                $e.attr(Attr_Src, url)
                $e.addClass(Class_Alternate_Pic_List_Enabled)
            })
        });
    }


    /**
     * setup links
     * @param {any} $from root object
     */
    setLinks($from) {

        var $t = $(
            Query_Select_Prefix
            + Data_HRef
            + Query_Select_Postfix,
            $from)

        $t.each((i, e) => {
            var $e = $(e)
            var href = $e.attr(Data_HRef)
            var target = $e.attr(Data_Target)
            $e.on(Event_Click, e => {
                if (!target)
                    window.location = href;
                else {
                    window.open(href, target)
                }
                e.stopPropagation()
            })
        })
    }

    /**
     * handle background image loaded event
     * @param {any} img
     */
    #handleBackImgLoaded(img) {
        var $i = $('#Image_Background')
        var w = img.naturalWidth
        var h = img.naturalHeight
        var $c = $('.movie-page-background-container')
        var wc = $c.width()
        var hc = $c.height()
        var maxw = w >= h
        var aw = w, ah = h

        var w0 = w
        var h0 = h
        while (w > wc && h > hc) {
            aw = w
            ah = h
            w /= 1.2
            h /= 1.2
        }
        w = aw
        h = ah
        var zoom = w / w0;

        var setwh = false
        if (w < wc) {
            var z = wc / w
            w *= z
            h *= z
            setwh = true
        }

        if (h < hc) {
            var z = hc / h
            w *= z
            h *= z
            setwh = true
        }

        var cl = maxw ?
            'width100p' : 'height100p'
        var left = w >= wc ?
            -(maxw ? w0 : w - wc) / 2 : (wc - w0) / 2
        var top = h >= hc ?
            -(!maxw ? h0 : h - hc) / 2 : (hc - h0) / 2

        $i.addClass(cl)
        $i.css('left', left + 'px')
        $i.css('top', top + 'px')
        $i.css('zoom', zoom)
        if (setwh) {
            $i.css('width', w + 'px')
            $i.css('height', h + 'px')
        }

        this.hideBarLoading()
        $i[0].src = img.src
        $i.fadeIn(1000)
    }

    /**
     * update background image size and position
     */
    #updateBackImgSizeAndPos()
    {

    }

    /**
    * handle the loading of the background image
    * @param {any} src
    */
    addBackImgLoadedHandler(src) {
        var img = new Image();
        img.addEventListener(
            Event_Load,
            () => this.#handleBackImgLoaded(img), false)
        img.addEventListener(
            Event_Error,
            () => this.hideBarLoading(), false)
        img.src = src;
    }

    /**
    * scroll to item in movie list
    */
    scrollToItemInMovieList() {
        const t = window.location.href.split(HRef_Id_Separator)
        if (t.length == 2) {
            const $it = $(Query_Equals_Id_Prefix + t[1] + Query_Selector_Postfix)
            const zoom = _ui().getZoomScale()
            if (zoom==0) zoom = 1
            var top = ($it.offset().top - $it.height())
                * 1/zoom
            $(Query_Prefix_Class + Class_Movie_List).scrollTop(top)
        }
    }

    /**
     * show the movie list (on movie list page only)
     */
    showList() {
        const $movieList = $('.movie-list')
        console.debug('show list')
        $movieList.fadeIn(List_FadeIn_Time)
        $('.icon-loading').fadeOut()
        this.scrollToItemInMovieList()
    }

    /**
     * show the movie details (on movie details page only)
     */
    showDetails() {
        const $movieDetails = $('.movie-details')
        console.debug('show details')
        $movieDetails.fadeIn(Details_FadeIn_Time)
        $('.icon-loading').fadeOut()
    }

    /**
     * hide bar loading
     */
    hideBarLoading() {
        $('.icon-bar-loading').fadeOut()
    }
}
