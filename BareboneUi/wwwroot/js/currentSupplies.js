$(function () {
    var currentSupplies;

    currentSupplies = ehl.currentSupplies;
    populateSuppliersDropDown("GasSupplier", currentSupplies.fuels.gas.suppliers);
    populateSuppliersDropDown("ElectricitySupplier", currentSupplies.fuels.electricity.suppliers);
    populateGasSupplierTariffsDropDown();
    populateElectricitySupplierTariffsDropDown();
    populateGasPaymentMethodsDropDown();
    populateElectricityPaymentMethodsDropDown();

    $("#GasSupplier").change(function () {
        populateGasSupplierTariffsDropDown();
    });

    $("#GasSupplierTariff").change(function () {
        populateGasPaymentMethodsDropDown();
    });

    $("#ElectricitySupplier").change(function () {
        populateElectricitySupplierTariffsDropDown();
    });

    $("#ElectricitySupplierTariff").change(function () {
        populateElectricityPaymentMethodsDropDown();
    });

    function populateSuppliersDropDown(dropDownId, suppliers) {
        var dropdown;

        dropdown = $("#" + dropDownId);
        $.each(suppliers,
            function () {
                dropdown.append($("<option />").val(this.id).text(this.name));
            });
    }

    function populateGasSupplierTariffsDropDown() {
        populateSupplierTariffsDropDown("GasSupplier", "GasSupplierTariff", currentSupplies.fuels.gas.suppliers);
        populateGasPaymentMethodsDropDown();
    }

    function populateElectricitySupplierTariffsDropDown() {
        populateSupplierTariffsDropDown("ElectricitySupplier",
            "ElectricitySupplierTariff",
            currentSupplies.fuels.electricity.suppliers);
        populateElectricityPaymentMethodsDropDown();
    }

    function populateSupplierTariffsDropDown(supplierDropDownId, supplierTariffDropDownId, suppliers) {
        var supplierId,
            supplier;

        supplierId = getSupplierId(supplierDropDownId);
        supplier = getSupplier(suppliers, supplierId);
        updateSupplierTariffs(supplierTariffDropDownId, supplier);
    }

    function updateSupplierTariffs(dropdownId, supplier) {
        var dropdown;

        dropdown = $("#" + dropdownId);
        dropdown.empty();
        $.each(supplier.supplierTariffs,
            function () {
                dropdown.append($("<option />").val(this.id).text(this.name));
            });
    }

    function populateGasPaymentMethodsDropDown() {
        populatePaymentMethodsDropDown("GasSupplier",
            "GasSupplierTariff",
            "GasSupplierPaymentMethod",
            currentSupplies.fuels.gas.suppliers);
    }

    function populateElectricityPaymentMethodsDropDown() {
        populatePaymentMethodsDropDown("ElectricitySupplier",
            "ElectricitySupplierTariff",
            "ElectricitySupplierPaymentMethod",
            currentSupplies.fuels.electricity.suppliers);
    }

    function populatePaymentMethodsDropDown(supplierDropDownId,
        supplierTariffDropDownId,
        paymentMethodDropDownId,
        suppliers) {
        var supplier,
            supplierId,
            supplierTariff,
            supplierTariffId,
            paymentMethodDropDown;

        supplierId = getSupplierId(supplierDropDownId);
        supplier = getSupplier(suppliers, supplierId);
        supplierTariffId = getSupplierTariffId(supplierTariffDropDownId);
        supplierTariff = _.find(supplier.supplierTariffs, { id: supplierTariffId });
        paymentMethodDropDown = $("#" + paymentMethodDropDownId);
        paymentMethodDropDown.empty();
        $.each(supplierTariff.paymentMethods,
            function () {
                var paymentMethod;

                paymentMethod = _.find(currentSupplies.paymentMethods, { id: this.id });
                paymentMethodDropDown.append($("<option />").val(paymentMethod.id).text(paymentMethod.name));
            });
    }

    function getSupplierId(supplierDropDownId) {
        return $("#" + supplierDropDownId).val();
    }

    function getSupplier(suppliers, supplierId) {
        return _.find(suppliers, { id: supplierId });
    }

    function getSupplierTariffId(supplierTariffDropDownId) {
        return $("#" + supplierTariffDropDownId).val();
    }
});