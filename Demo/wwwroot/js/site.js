'use strict';
$(document)
    .ready(function () {
        $(document)
            .on("click",
                ".remove-rule-button",
                function (event) { // ensures the handler applies to the ajax-loaded buttons, too
                    var params = { ruleName: $(this).data("rule-name") };

                    $.ajax({
                        type: "POST",
                        url: $(this).data("post-url"),
                        data: params
                    })
                        .done(function (data) {
                            $("#rules_container").html(data);
                        });
                }
            );
        $(document)
            .on("click",
                ".remove-group-button",
                function (event) { // ensures the handler applies to the ajax-loaded buttons, too
                    var params = { groupName: $(this).data("group-name") };

                    $.ajax({
                        type: "POST",
                        url: $(this).data("post-url"),
                        data: params
                    })
                        .done(function (data) {
                            $("#groups_container").html(data);
                        });
                }
            );
        $(".rules-refresh")
            .click(function (event) {
                $.ajax({
                    type: "GET",
                    url: $(this).data("get-url"),
                })
                    .done(function (data) {
                        $("#rules_container").html(data);
                    });
            }
            );
        $(".groups-refresh")
            .click(function (event) {
                $.ajax({
                    type: "GET",
                    url: $(this).data("get-url"),
                })
                    .done(function (data) {
                        $("#groups_container").html(data);
                    });
            }
            );

        $("#create_rule")
            .submit(function (event) {
                event.preventDefault();
                var params = {
                    code: $("#createRule_RuleCode").val(),
                    group: $("#createRule_GroupCode").val(),
                    source: $("#createRule_Source").val()
                };

                $.ajax({
                    type: "POST",
                    url: $(this).prop("target"),
                    data: params
                })
                    .fail(function (request_object, data, error) {
                        alert("Rule submission failed: " + request_object.status + "; " + "Non-existant group?");
                    })
                    .done(function (data) {
                        $("#rules_container").html(data);
                        $("#createRule_RuleCode").val("");
                        $("#createRule_GroupCode").val("");
                        $("#createRule_Source").val("");
                    });
            });

        $("#create_group")
            .submit(function (event) {
                event.preventDefault();
                var params = {
                    name: $("#createGroup_GroupName").val(),
                    parameters: gatherGroupParameters()
                };

                $.ajax({
                    type: "POST",
                    url: $(this).prop("target"),
                    data: params
                })
                    .fail(function (request_object, data, error) {
                        alert("Group submission failed");
                    })
                    .done(function (data) {
                        $("#groups_container").html(data);
                        $("#createGroup_GroupName").val("");
                        $("#createGroup_Parameters > tbody > tr").remove();
                    });
            });
        $("#run-submit")
            .submit(function (event) {
                event.preventDefault();
                $("#rule_results").html("");

                var params = {
                    group: $("#runRules_GroupCode").val(),
                    parameters: gatherParameters()
                };

                $.ajax({
                    type: "POST",
                    url: $(this).prop("target"),
                    data: params
                })
                    .done(function (data) {
                        $("#runRules_GroupCode").val("");
                        $("#runRules_Parameters").find("tr:gt(0)").remove(); // all but header
                        $("#rule_results").html(data);
                    })
                    .fail(function (request_object, data, error) {
                        alert("Rule submission failed: " + request_object.status + "; " + "Non-existant group?");
                    });
            });

        $(".run-parameters-append")
            .click(function (event) {
                $("#runRules_Parameters").append($("#parameter_model").html());
            });

        $(".group-parameters-append")
            .click(function (event) {
                $("#createGroup_Parameters").append($("#createGroup_ParameterModel").html());
            });

        $(document)
            .on("click",
                ".parameter-remove",
                function (data) {
                    $(this).parent().parent().remove();
                });

        $(document)
            .on("click",
                ".edit-rule-button",
                function (event) {
                    var elems = $(this).parent().parent().parent().find("td").slice(0, 3);
                    var values = [];
                    elems.each(function (i, e) {
                        console.log(e + ";" + i);
                        values[i] = e.innerHTML;
                    });

                    $("#createRule_RuleCode").val(values[0]);
                    $("#createRule_GroupCode").val(values[1]);
                    $("#createRule_Source").val(values[2]);
                });


        function gatherParameters() {
            var params = $("#runRules_Parameters .parameter");
            var res = [];
            params.each(function (e) {
                res.push(// figure out a better way to do this!
                    $(this).children(":first").first().children().first().val()
                );
            });
            return res;
        }

        function gatherGroupParameters() {
            var params = $("#createGroup_Parameters tbody > .group-parameter");
            var res = [];
            params.each(function (e) {
                var inputs = $("#createGroup_Parameters tbody > .group-parameter").first().find("input");
                res.push({
                    Name: inputs.first().val(),
                    FullyQualifiedTypeName: inputs.last().val()
                });
            });
            return res;
        }

    });
