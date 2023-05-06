import{a as t}from"./_tslib-158249d5.js";import{D as e,a as i,b as r,I as o}from"./popup-79e4f7d5.js";import{B as s}from"./popuproot-d37b16a2.js";import{ThumbDragStartedEvent as a,ThumbDragDeltaEvent as d,ThumbDragCompletedEvent as n}from"./thumb-b78dcc42.js";import{P as l}from"./point-9c6ab88a.js";import{R as h,a as p}from"./rect-7fc5c2ef.js";import{L as c,D as g}from"./layouthelper-4b19d191.js";import{E as m}from"./eventhelper-8570b930.js";import{C as u}from"./custom-events-helper-18f7786a.js";import{i as b}from"./query-44b9267f.js";import{e as f}from"./property-d3853089.js";import{n as x}from"./custom-element-bd7061f2.js";import{$ as y,s as v,r as C}from"./lit-element-b0a6fcba.js";import{D}from"./popupbasedialog-0778f080.js";import{dxBranchTagName as w}from"./branch-ea431ccc.js";import{D as S}from"./popupportal-447465dc.js";import{d as H}from"./events-interseptor-4336a3f0.js";import"./rafaction-bba7928b.js";import"./transformhelper-ebad0156.js";import"./positiontracker-dba18a16.js";import"./elementobserver-5f004683.js";import"./supportcaptureelement-35919fa4.js";import"./logicaltreehelper-15991dcb.js";import"./dx-ui-element-de378e9d.js";import"./lit-element-base-af247167.js";import"./data-qa-utils-8be7c726.js";import"./capturemanager-c7d5aef8.js";import"./simpleevent-84045703.js";import"./locker-c40478e6.js";import"./nameof-factory-64d95f5b.js";class R{constructor(t,e){this.width=Math.floor(t),this.height=Math.floor(e)}}class j extends CustomEvent{constructor(t,e){super(j.eventName,{detail:new R(t,e),bubbles:!0,composed:!0,cancelable:!0})}}j.eventName="dxbl-dropdown.resizeStarted",u.register(j.eventName,(t=>t.detail));class z extends CustomEvent{constructor(t,e){super(z.eventName,{detail:new R(t,e),bubbles:!0,composed:!0,cancelable:!0})}}z.eventName="dxbl-dropdown.resizeCompleted",u.register(z.eventName,(t=>t.detail));let W=class extends e{constructor(){super(...arguments),this.dragStart=null,this.dragBounds=null,this.dragStartedHandler=this.handleDragStarted.bind(this),this.dragDeltaHandler=this.handleDragDelta.bind(this),this.dragCompletedHandler=this.handleDragCompleted.bind(this),this.dragWidth=0,this.dragHeight=0,this.dragMaxWidth=0,this.dragMaxHeight=0,this.isInDragOperation=!1,this.dragCssStyle=null}get branchType(){return s.DropDown}renderTemplate(){return y`
            <dxbl-branch
                id="branch"
                branch-id="${this.branchId}"
                parent-branch-id="${this.parentBranchId}"
                type="${this.branchType}"
                style="${this.dragCssStyle}">
                <dxbl-dropdown-root
                    id="root"
                    style="${this.rootCssStyle}"
                    ?resizing="${this.resizing}"
                    drop-opposite="${this.actualDropOpposite}"
                    drop-direction="${this.actualDropDirection}"
                    drop-alignment="${this.actualDropAlignment}">
                    ${this.renderDefaultSlot()}
                    ${this.renderAdditionalSlots()}
                    ${this.renderBridgeSlot()}
                </dxbl-dropdown-root>
            </dxbl-branch>
        `}connectedCallback(){super.connectedCallback(),this.addEventListener(a.eventName,this.dragStartedHandler),this.addEventListener(d.eventName,this.dragDeltaHandler),this.addEventListener(n.eventName,this.dragCompletedHandler)}disconnectedCallback(){super.disconnectedCallback(),this.removeEventListener(a.eventName,this.dragStartedHandler),this.removeEventListener(d.eventName,this.dragDeltaHandler),this.removeEventListener(n.eventName,this.dragCompletedHandler)}get child(){return this.root}handleDragStarted(t){const e=m.getOriginalSource(t);if(!e)return;if(!e.hasAttribute("data-dropdown-thumb"))return;if(!this.child)return;this.lockPlacement(),this.dragStart=new l(t.detail.x,t.detail.y);const i=this.getRestrictBounds(),r=this.getPlacementTargetInterestPoints(this.placement),o=this.actualDropAlignment,s=this.actualDropDirection,a=c.getRelativeElementRect(this.child);this.dragBounds=this.calcResizeRect(i,r,o,s),this.dragMaxWidth=this.dragBounds.width,this.dragMaxHeight=this.dragBounds.height,this.isInDragOperation=!0,this.dragWidth=a.width,this.dragHeight=a.height,this.raiseResizeStarted(this.dragWidth,this.dragHeight)}willUpdate(t){super.willUpdate(t),this.dragCssStyle=this.isInDragOperation?`width: ${g.toPx(this.dragWidth)}; height: ${g.toPx(this.dragHeight)}; max-width: ${g.toPx(this.dragMaxWidth)}; max-height: ${g.toPx(this.dragMaxHeight)};`:null}updated(t){super.updated(t),this.root.offsetWidth<this.root.scrollWidth&&(this.dragWidth=this.root.scrollWidth),this.root.offsetHeight<this.root.scrollHeight&&(this.dragHeight=this.root.scrollHeight)}handleDragDelta(t){const e=m.getOriginalSource(t);e&&e.hasAttribute("data-dropdown-thumb")&&(this.dragWidth=Math.min(this.dragMaxWidth,this.actualDropDirection===i.Near?t.detail.x-this.offset.x:this.offset.x+this.childSize.width-t.detail.x),this.dragHeight=Math.min(this.dragMaxHeight,this.actualDropAlignment===r.bottom?t.detail.y-this.offset.y:this.offset.y+this.childSize.height-t.detail.y))}handleDragCompleted(t){var e,i;const r=m.getOriginalSource(t);r&&r.hasAttribute("data-dropdown-thumb")&&(this.isInDragOperation=!1,this.dragWidth=null!==(e=this.branch.offsetWidth)&&void 0!==e?e:0,this.dragHeight=null!==(i=this.branch.offsetHeight)&&void 0!==i?i:0,this.desiredWidth=g.toPx(this.dragWidth),this.desiredHeight=g.toPx(this.dragHeight),this.unlockPlacement(),this.raiseResizeCompleted(this.dragWidth,this.dragHeight))}calcResizeRect(t,e,s,a){if(a===i.Near){if(s===r.top){return h.intersect(t,p.createFromPoints(e[o.TopLeft],t.topRight))}return h.intersect(t,p.createFromPoints(e[o.BottomLeft],t.bottomRight))}if(s===r.top){return h.intersect(t,p.createFromPoints(e[o.TopRight],t.topLeft))}return h.intersect(t,p.createFromPoints(e[o.BottomRight],t.bottomLeft))}raiseResizeStarted(t,e){this.dispatchEvent(new j(t,e))}raiseResizeCompleted(t,e){this.dispatchEvent(new z(t,e))}calcRenderWidth(){return this.isInDragOperation?null:super.calcRenderWidth()}calcRenderHeight(){return this.isInDragOperation?null:super.calcRenderHeight()}shouldUpdateRootCssStyle(t){return super.shouldUpdateRootCssStyle(t)||t.has("isInDragOperation")||t.has("dragWidth")||t.has("dragHeight")||t.has("dragMaxWidth")||t.has("dragMaxHeight")}};t([b("#root",!0)],W.prototype,"root",void 0),t([b("#branch")],W.prototype,"branch",void 0),t([f({type:Number,reflect:!1})],W.prototype,"dragWidth",void 0),t([f({type:Number,reflect:!1})],W.prototype,"dragHeight",void 0),t([f({type:Number,reflect:!1})],W.prototype,"dragMaxWidth",void 0),t([f({type:Number,reflect:!1})],W.prototype,"dragMaxHeight",void 0),t([f({type:Boolean,reflect:!1})],W.prototype,"isInDragOperation",void 0),t([f({type:String,reflect:!1})],W.prototype,"dragCssStyle",void 0),W=t([x("dxbl-dropdown")],W);let N=class extends D{createRenderRoot(){return this}};N=t([x("dxbl-dropdown-dialog")],N);let $=class extends v{constructor(){super(...arguments),this.topLeftClass=null,this.topRightClass=null,this.bottomLeftClass=null,this.bottomRightClass=null,this.dropOpposite=!1,this.dropDirection=i.Near,this.dropAlignment=r.bottom,this.resizing=!1}static get styles(){return C`
            :host {
                display: flex;
                position: relative;
                flex: 1 1 auto;
                flex-direction: column;
                box-sizing: border-box;
                min-height: 0;
            }
            .hidden {
                display: none;
            }
            ::slotted([slot="top-right"]) {
                position: absolute;
                z-index: 1001;
                top: 0px;
                right: 0px;
                transform: rotate(270deg);
                cursor: ne-resize;
            }
            ::slotted([slot="top-left"]) {
                position: absolute;
                z-index: 1001;
                top: 0px;
                left: 0px;
                transform: rotate(180deg);
                cursor: nw-resize;
            }
            ::slotted([slot="bottom-left"]) {
                position: absolute;
                z-index: 1001;
                bottom: 0px;
                left: 0px;
                transform: rotate(90deg);
                cursor: sw-resize;
            }
            ::slotted([slot="bottom-right"]) {
                position: absolute;
                z-index: 1001;
                bottom: 0px;
                right: 0px;
                cursor: se-resize;
                transform: rotate(0deg);
            }
        }`}connectedCallback(){super.connectedCallback(),this.calculateStyles(this.resizing,this.dropAlignment,this.dropDirection)}willUpdate(t){(t.has("dropAlignment")||t.has("dropDirection")||t.has("resizing"))&&this.calculateStyles(this.resizing,this.dropAlignment,this.dropDirection)}calculateStyles(t,e,o){this.topLeftClass=t&&e===r.top&&o===i.Far?null:"hidden",this.topRightClass=t&&e===r.top&&o===i.Near?null:"hidden",this.bottomLeftClass=t&&e===r.bottom&&o===i.Far?null:"hidden",this.bottomRightClass=t&&e===r.bottom&&o===i.Near?null:"hidden"}render(){return y`
            <slot></slot>
            <dxbl-thumb data-qa-thumb-location="top-left" data-dropdown-thumb class="${this.topLeftClass}">
                <slot name="top-left"></slot>
            </dxbl-thumb>
            <dxbl-thumb data-qa-thumb-location="top-right" data-dropdown-thumb class="${this.topRightClass}">
                <slot name="top-right"></slot>
            </dxbl-thumb>
            <dxbl-thumb data-qa-thumb-location="bottom-left" data-dropdown-thumb class="${this.bottomLeftClass}">
                <slot name="bottom-left"></slot>
            </dxbl-thumb>
            <dxbl-thumb data-qa-thumb-location="bottom-right" data-dropdown-thumb class="${this.bottomRightClass}">
                <slot name="bottom-right"></slot>
            </dxbl-thumb>`}};t([f({type:String,reflect:!1})],$.prototype,"topLeftClass",void 0),t([f({type:String,reflect:!1})],$.prototype,"topRightClass",void 0),t([f({type:String,reflect:!1})],$.prototype,"bottomLeftClass",void 0),t([f({type:String,reflect:!1})],$.prototype,"bottomRightClass",void 0),t([f({type:Object,attribute:"drop-opposite"})],$.prototype,"dropOpposite",void 0),t([f({type:String,attribute:"drop-direction"})],$.prototype,"dropDirection",void 0),t([f({type:String,attribute:"drop-alignment"})],$.prototype,"dropAlignment",void 0),t([f({type:Boolean,attribute:"resizing"})],$.prototype,"resizing",void 0),$=t([x("dxbl-dropdown-root")],$);const L=["dxbl-dropdown","dxbl-dropdown-dialog","dxbl-dropdown-root",w,S,H];function M(t){if(!t)throw new Error("failed");return t}const O={getReference:M,registeredComponents:L};export{O as default,M as getReference,L as registeredComponents};
