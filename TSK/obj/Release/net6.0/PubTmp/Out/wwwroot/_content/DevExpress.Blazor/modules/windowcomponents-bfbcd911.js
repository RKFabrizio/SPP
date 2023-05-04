import{a as t}from"./_tslib-158249d5.js";import{D as o,a as e}from"./popup-79e4f7d5.js";import{B as s,a as r}from"./popuproot-d37b16a2.js";import{i}from"./query-44b9267f.js";import{n as l}from"./custom-element-bd7061f2.js";import{$ as p,s as a,r as d}from"./lit-element-b0a6fcba.js";import{D as n}from"./popupbasedialog-0778f080.js";import{e as m}from"./property-d3853089.js";import{dxBranchTagName as b}from"./branch-ea431ccc.js";import"./rect-7fc5c2ef.js";import"./point-9c6ab88a.js";import"./rafaction-bba7928b.js";import"./transformhelper-ebad0156.js";import"./layouthelper-4b19d191.js";import"./positiontracker-dba18a16.js";import"./elementobserver-5f004683.js";import"./supportcaptureelement-35919fa4.js";import"./logicaltreehelper-15991dcb.js";import"./dx-ui-element-de378e9d.js";import"./lit-element-base-af247167.js";import"./data-qa-utils-8be7c726.js";import"./capturemanager-c7d5aef8.js";import"./simpleevent-84045703.js";import"./locker-c40478e6.js";import"./eventhelper-8570b930.js";import"./nameof-factory-64d95f5b.js";import"./custom-events-helper-18f7786a.js";let h=class extends o{renderTemplate(){return p`
            <dxbl-branch
                id="${this.branchId}"
                parent-id="${this.parentBranchId}"
                type="${s.Window}">
                <dxbl-window-root
                    id="root"
                    style="${this.rootCssStyle}"
                    resizing="${this.resizing}"
                    drop-opposite="${this.actualDropOpposite}"
                    drop-direction="${this.actualDropDirection}">
                    ${this.renderSlot()}
                </dxbl-window-root>
            </dxbl-branch>
        `}get child(){return this.root}renderSlot(){return p`
            <slot></slot>
            <slot name="top" slot="top"></slot>
            <slot name="right" slot="right"></slot>
            <slot name="bottom" slot="bottom"></slot>
            <slot name="left" slot="left"></slot>
            <slot name="top-left" slot="top-left"></slot>
            <slot name="top-right" slot="top-right"></slot>
            <slot name="bottom-left" slot="bottom-left"></slot>
            <slot name="bottom-right" slot="bottom-right"></slot>
        `}};t([i("#root",!0)],h.prototype,"root",void 0),h=t([l("dxbl-window")],h);let c=class extends n{};c=t([l("dxbl-dropdown-dialog")],c);let u=class extends a{constructor(){super(...arguments),this.topLeftClass=null,this.topRightClass=null,this.bottomLeftClass=null,this.bottomRightClass=null,this.dropOpposite=!1,this.dropDirection=e.Near,this.resizing=!1}static get styles(){return d`
            :host {
                display: flex;
                box-sizing: border-box;
            }
            .hidden {
                display: none;
            }
            ::slotted {
                min-width: 8px;
                min-height: 8px;
            }
            ::slotted([slot="top-left"]) {
                position: absolute;
                z-index: 1;
                top: 0px;
                left: 0px;
                transform: rotate(-90deg);
                cursor: nw-resize;
            }
            ::slotted([slot="top-right"]) {
                position: absolute;
                z-index: 1;
                top: 0px;
                right: 0px;
                transform: rotate(-180deg);
                cursor: ne-resize;
            }
            ::slotted([slot="bottom-left"]) {
                position: absolute;
                z-index: 1;
                bottom: 0px;
                left: 0px;
                transform: rotate(-270deg);
                cursor: sw-resize;
            }
            ::slotted([slot="bottom-right"]) {
                position: absolute;
                z-index: 1;
                bottom: 0px;
                right: 0px;
                cursor: se-resize;
                transform: rotate(0deg);
            }
        }`}connectedCallback(){super.connectedCallback(),this.calculateStyles(this.resizing,this.dropOpposite,this.dropDirection)}willUpdate(t){(t.has("dropOpposite")||t.has("dropDirection")||t.has("resizing"))&&this.calculateStyles(this.resizing,this.dropOpposite,this.dropDirection)}calculateStyles(t,o,s){this.topLeftClass=t&&o&&s===e.Far?null:"hidden",this.topRightClass=t&&o&&s===e.Near?null:"hidden",this.bottomLeftClass=t&&!o&&s===e.Far?null:"hidden",this.bottomRightClass=t&&!o&&s===e.Near?null:"hidden"}render(){return p`
            <slot></slot>
            <dxbl-thumb>
                <slot name="top"></slot>
            </dxbl-thumb>
            <dxbl-thumb>
                <slot name="right"></slot>
            </dxbl-thumb>
            <dxbl-thumb>
                <slot name="bottom"></slot>
            </dxbl-thumb>
            <dxbl-thumb>
                <slot name="left"></slot>
            </dxbl-thumb>
            <dxbl-thumb class="${this.topLeftClass}">
                <slot name="top-left"></slot>
            </dxbl-thumb>
            <dxbl-thumb class="${this.topRightClass}">
                <slot name="top-right"></slot>
            </dxbl-thumb>
            <dxbl-thumb class="${this.bottomLeftClass}">
                <slot name="bottom-left"></slot>
            </dxbl-thumb>
            <dxbl-thumb class="${this.bottomRightClass}">
                <slot name="bottom-right"></slot>
            </dxbl-thumb>`}};t([m({type:String,reflect:!1})],u.prototype,"topLeftClass",void 0),t([m({type:String,reflect:!1})],u.prototype,"topRightClass",void 0),t([m({type:String,reflect:!1})],u.prototype,"bottomLeftClass",void 0),t([m({type:String,reflect:!1})],u.prototype,"bottomRightClass",void 0),t([m({type:Object,attribute:"drop-opposite"})],u.prototype,"dropOpposite",void 0),t([m({type:String,attribute:"drop-direction"})],u.prototype,"dropDirection",void 0),t([m({type:Object,attribute:"resizing"})],u.prototype,"resizing",void 0),u=t([l("dxbl-window-root")],u);function f(t){if(!t)throw new Error("failed");return t}const g={getReference:f,registeredComponents:["dxbl-window","dxbl-dropdown-dialog",r,"dxbl-window-root",b]};export{g as default,f as getReference};
