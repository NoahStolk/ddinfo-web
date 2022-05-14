window.outsideClickHandler = {
	addEvent: function (elementId, dotNetObjectReference) {
		window.addEventListener("click", (e) => {
			const element = document.getElementById(elementId);
			if (!element) {
				return;
			}

			if (!element.contains(e.target)) {
				dotNetObjectReference.invokeMethodAsync("InvokeClickOutside");
			}
		});
	}
};
