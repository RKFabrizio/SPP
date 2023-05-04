import{D as e}from"./data-qa-utils-8be7c726.js";import{s as t,r as s,$ as a}from"./lit-element-b0a6fcba.js";class l extends t{static get styles(){return s`
            :host {
                display: flex;
            }
        `}connectedCallback(){super.connectedCallback(),e.setLoaded(this)}disconnectedCallback(){super.disconnectedCallback(),e.removeLoaded(this)}render(){return a`<slot></slot>`}}export{l as D};
