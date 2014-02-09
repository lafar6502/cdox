
Ext.define('CogDox.ui.PagingToolbar', {
    extend: 'Ext.container.Container',
    data: {},
    tpl: ['<div class="row">',
        '<div class="span6 pagination">',
        '<ul>',
        '<li class="prev"><a href="#">← <span class="hidden-480">Previous</span></a></li>',
        '<li class="next"><a href="#"><span class="hidden-480">Next</span> → </a></li>',
        '</ul>',
        '</div>',
        '</div>'],
    
    initComponent: function () {
        this.callParent(arguments);
    }
});

//grid panel for cogdox list
Ext.define('CogDox.ui.BaseListGrid', {
    extend: "Ext.grid.Panel",
    listId: null,
    storeConfig: null,
    requires: [
        
    ],
    pageSize: 10,
    tbarItems: [],
    dockedItems: null,
    initComponent: function() {
        var me = this;
        if (Ext.isEmpty(this.store)) {
            if (!Ext.isEmpty(this.listId)) 
            {
                this.store = CogDox.ui.ListDataStores.createStore(this.listId, {pageSize: this.pageSize});
            }
        }
        if (Ext.isEmpty(this.store)) throw "Store missing";
        this.store.getProxy().extraParams = {"ala" : "ma kota", "aquery" : "jest takie"};
        if (null == this.dockedItems) {
            this.dockedItems = [
                /*Ext.create('CogDox.ui.PagingToolbar', {
                    store: this.store,
                    dock: 'top'
                })*/
                {
                    xtype: 'pagingtoolbar',
                    store: this.store,   
                    dock: 'top',
                    displayInfo: false, 
                    items: [
                        {xtype: 'tbtext', itemId: 'displayItem'}
                    ].concat(Ext.isEmpty(this.tbarItems) ? [] : this.tbarItems)
                }
            ]
            if (!Ext.isEmpty(this.tbarItems)) {
                //this.dockedItems[0].items.push(this.tbarItems);
            }
        }
        this.callParent(arguments);
    }
});

Ext.define('CogDox.ui.ListFilterPanel', {
    extend: 'Ext.panel.Panel',
    requires: [],
    header: false,
    collapseMode: 'mini',
    title: null,
    getCurrentFilter: function() {
    },
    loadFilter: function(flt) {
    },
    initComponent: function() {
        this.callParent(arguments);
    }
});


Ext.define('CogDox.ui.SearchGrid', {
    extend: 'Ext.panel.Panel',
    requires: [],
    layout: 'border',
    listId: '',
    searchPanelClass: null,
    searchPanelConfig: {},
    listGridClass: null,
    listGridConfig: {},
    showSearchPanel: function() {
        var p = this.getComponent('searchPnl');
        if (p.getCollapsed())
            p.expand();
        else
            p.collapse();
    },
    buildFilterMenu: function() {
        Ext.Ajax.request({
            url: CogDox.ui.Config.BASE_URL + "/List/GetListFilters",
            params: {listId: this.listId},
            success: function(resp, opts) {
                var lst = Ext.JSON.decode(resp.responseText);
            }
        });
    },
    initComponent: function() {
        var me = this;
        var filterMnu = Ext.create('Ext.menu.Menu', {
            items: [{text: 'a ja to co?'}]
        });
        var fillMnu = function() {
            console.log('adding menu item');
            filterMnu.add(Ext.create('Ext.menu.Item', {text: 'Dodali mnie!'}));
            filterMnu.un('activate', fillMnu);
        };
        filterMnu.on('activate', fillMnu);
        
        var gridCfg = {
            region: 'center', itemId: 'theGrid', 
            tbarItems: [
                {xtype: 'tbfill'},
                {
                    text: 'Filtry', menu: filterMnu
                },
                {
                    xtype: 'button', text: '<<', border: true,
                    handler: function() {
                        me.showSearchPanel();
                    }
                }
            ],
            listeners: {
                itemclick: function(p, i) {
                    me.fireEvent('itemclick', me, i);
                }
            }
        };
        Ext.apply(gridCfg, this.listGridConfig);
        var its = [
            Ext.create(this.listGridClass, gridCfg)
        ];
        if (!Ext.isEmpty(this.searchPanelClass)) {
            its.push(Ext.create(this.searchPanelClass, Ext.apply(this.searchPanelConfig, {
                region: 'east', itemId: 'searchPnl', border: false, collapsible: true, 
                collapsed: true
            })));
        }
        this.items = its;
        this.addEvents('itemclick', 'itemrightclick');
        this.callParent(arguments);
    }
});



