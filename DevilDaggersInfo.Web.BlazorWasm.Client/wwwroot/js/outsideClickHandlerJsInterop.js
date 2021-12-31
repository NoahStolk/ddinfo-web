window.outsideClickHandler = {
	addEvent: function (elementId, dotNetObjectReference) {
		window.addEventListener("click", (e) => {
			if (!document.getElementById(elementId).contains(e.target)) {
				dotNetObjectReference.invokeMethodAsync("InvokeClickOutside");
			}
		});
	}
};
