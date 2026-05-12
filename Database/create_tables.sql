-- =============================================================================
-- Change Point System - PostgreSQL Schema
-- Schema: change_point
-- External employee table: {mainSchema}.center_tm_employee
-- =============================================================================

CREATE SCHEMA IF NOT EXISTS change_point;

SET search_path TO change_point;

-- =============================================================================
-- CP_GROUP — factory work groups
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_group (
    id          SERIAL PRIMARY KEY,
    name        VARCHAR(200)    NOT NULL,
    wc_list     TEXT,
    is_use      BOOLEAN         NOT NULL DEFAULT TRUE,
    menu_1      VARCHAR(10)     DEFAULT 'Y',
    menu_2      VARCHAR(10)     DEFAULT 'Y',
    menu_3      VARCHAR(10)     DEFAULT 'Y',
    menu_4      VARCHAR(10)     DEFAULT 'Y',
    menu_5      VARCHAR(10)     DEFAULT 'Y',
    color_1     VARCHAR(20)     DEFAULT '#ffffff',
    color_2     VARCHAR(20)     DEFAULT '#ffffff',
    color_3     VARCHAR(20)     DEFAULT '#ffffff',
    color_4     VARCHAR(20)     DEFAULT '#ffffff',
    color_5     VARCHAR(20)     DEFAULT '#ffffff',
    create_by   VARCHAR(20),
    create_date TIMESTAMPTZ     DEFAULT NOW(),
    update_by   VARCHAR(20),
    update_date TIMESTAMPTZ     DEFAULT NOW()
);

-- =============================================================================
-- CP_GROUP_WC — group ↔ work center mapping
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_group_wc (
    id          SERIAL PRIMARY KEY,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    wc_code     VARCHAR(20)     NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_cp_group_wc_group ON change_point.cp_group_wc(group_id);
CREATE INDEX IF NOT EXISTS idx_cp_group_wc_wc    ON change_point.cp_group_wc(wc_code);

-- =============================================================================
-- CP_CALENDAR — main change point records
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_calendar (
    id                    SERIAL PRIMARY KEY,
    group_id              INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    date_change           TIMESTAMPTZ,
    shift                 VARCHAR(10),
    shift_team            VARCHAR(10),
    man_spot              VARCHAR(20),
    man_instead           VARCHAR(20),
    man_spot_name_eng     VARCHAR(200),
    man_spot_name_tha     VARCHAR(200),
    man_instead_name_eng  VARCHAR(200),
    man_instead_name_tha  VARCHAR(200),
    mc_no                 VARCHAR(100),
    edit_point            VARCHAR(100),
    part_no               VARCHAR(100),
    mold_no               VARCHAR(100),
    change                VARCHAR(100),
    process_point         VARCHAR(100),
    details               INTEGER[]       DEFAULT '{}',
    action                TEXT[]          DEFAULT '{}',
    warnings              TEXT[]          DEFAULT '{}',
    status_type           VARCHAR(20)     DEFAULT 'show',
    remark                TEXT,
    informed              BOOLEAN         DEFAULT FALSE,
    recipient             INTEGER[]       DEFAULT '{}',
    type                  VARCHAR(20),
    create_by             VARCHAR(20),
    create_date           TIMESTAMPTZ     DEFAULT NOW(),
    update_by             VARCHAR(20),
    update_date           TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_calendar_group      ON change_point.cp_calendar(group_id);
CREATE INDEX IF NOT EXISTS idx_cp_calendar_date       ON change_point.cp_calendar(date_change);
CREATE INDEX IF NOT EXISTS idx_cp_calendar_create_by  ON change_point.cp_calendar(create_by);
CREATE INDEX IF NOT EXISTS idx_cp_calendar_status     ON change_point.cp_calendar(status_type);

-- =============================================================================
-- CP_FILES — uploaded files
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_files (
    id          SERIAL PRIMARY KEY,
    calendar_id INTEGER         DEFAULT 0,
    group_id    INTEGER,
    name        VARCHAR(500)    NOT NULL,
    file_type   VARCHAR(100),
    file_size   VARCHAR(50),
    create_date TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_files_calendar ON change_point.cp_files(calendar_id);
CREATE INDEX IF NOT EXISTS idx_cp_files_group    ON change_point.cp_files(group_id);

-- =============================================================================
-- CP_APPROVE_MEMBER — inspector groups (titles)
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_approve_member (
    id          SERIAL PRIMARY KEY,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    title       VARCHAR(200)    NOT NULL,
    member_list TEXT,
    is_use      BOOLEAN         NOT NULL DEFAULT TRUE,
    create_by   VARCHAR(20),
    create_date TIMESTAMPTZ     DEFAULT NOW(),
    update_by   VARCHAR(20),
    update_date TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_approve_member_group ON change_point.cp_approve_member(group_id);

-- =============================================================================
-- CP_APPROVE_MEMBER_ITEM — inspector group members
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_approve_member_item (
    id                 SERIAL PRIMARY KEY,
    approve_member_id  INTEGER         NOT NULL REFERENCES change_point.cp_approve_member(id) ON DELETE CASCADE,
    emp_no             VARCHAR(20)     NOT NULL,
    group_id           INTEGER
);

CREATE INDEX IF NOT EXISTS idx_cp_approve_member_item_mid ON change_point.cp_approve_member_item(approve_member_id);
CREATE INDEX IF NOT EXISTS idx_cp_approve_member_item_emp ON change_point.cp_approve_member_item(emp_no);

-- =============================================================================
-- CP_CALENDAR_CONFIRM — per-person confirmation of each CP
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_calendar_confirm (
    id               SERIAL PRIMARY KEY,
    calendar_id      INTEGER         NOT NULL REFERENCES change_point.cp_calendar(id) ON DELETE CASCADE,
    group_member_id  INTEGER,
    emp_no           VARCHAR(20)     NOT NULL,
    is_confirm       VARCHAR(5)      DEFAULT 'N',
    confirm_date     TIMESTAMPTZ,
    create_date      TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_calendar_confirm_cal ON change_point.cp_calendar_confirm(calendar_id);
CREATE INDEX IF NOT EXISTS idx_cp_calendar_confirm_emp ON change_point.cp_calendar_confirm(emp_no);

-- =============================================================================
-- CP_MOLD_NO — mold numbers lookup
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_mold_no (
    id          SERIAL PRIMARY KEY,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    name        VARCHAR(200)    NOT NULL,
    is_use      BOOLEAN         NOT NULL DEFAULT TRUE,
    create_by   VARCHAR(20),
    create_date TIMESTAMPTZ     DEFAULT NOW(),
    update_by   VARCHAR(20),
    update_date TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_mold_no_group ON change_point.cp_mold_no(group_id);

-- =============================================================================
-- CP_PROCESS — process lookup
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_process (
    id          SERIAL PRIMARY KEY,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    name        VARCHAR(200)    NOT NULL,
    is_use      BOOLEAN         NOT NULL DEFAULT TRUE,
    create_by   VARCHAR(20),
    create_date TIMESTAMPTZ     DEFAULT NOW(),
    update_by   VARCHAR(20),
    update_date TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_process_group ON change_point.cp_process(group_id);

-- =============================================================================
-- CP_EDIT — edit point lookup
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_edit (
    id          SERIAL PRIMARY KEY,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    name        VARCHAR(200)    NOT NULL,
    is_use      BOOLEAN         NOT NULL DEFAULT TRUE,
    create_by   VARCHAR(20),
    create_date TIMESTAMPTZ     DEFAULT NOW(),
    update_by   VARCHAR(20),
    update_date TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_edit_group ON change_point.cp_edit(group_id);

-- =============================================================================
-- CP_CHANGE — change type lookup
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_change (
    id          SERIAL PRIMARY KEY,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    name        VARCHAR(200)    NOT NULL,
    is_use      BOOLEAN         NOT NULL DEFAULT TRUE,
    create_by   VARCHAR(20),
    create_date TIMESTAMPTZ     DEFAULT NOW(),
    update_by   VARCHAR(20),
    update_date TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_change_group ON change_point.cp_change(group_id);

-- =============================================================================
-- CP_MACHINE — machine lookup
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_machine (
    id          SERIAL PRIMARY KEY,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    machine_no  VARCHAR(100)    NOT NULL,
    size_ton    VARCHAR(50),
    is_use      BOOLEAN         NOT NULL DEFAULT TRUE,
    create_by   VARCHAR(20),
    create_date TIMESTAMPTZ     DEFAULT NOW(),
    update_by   VARCHAR(20),
    update_date TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_machine_group ON change_point.cp_machine(group_id);

-- =============================================================================
-- CP_PART — part number lookup
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_part (
    part_no     VARCHAR(50)     NOT NULL,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    part_name   VARCHAR(200),
    is_use      BOOLEAN         NOT NULL DEFAULT TRUE,
    create_by   VARCHAR(20),
    create_date TIMESTAMPTZ     DEFAULT NOW(),
    update_by   VARCHAR(20),
    update_date TIMESTAMPTZ     DEFAULT NOW(),
    PRIMARY KEY (part_no, group_id)
);

CREATE INDEX IF NOT EXISTS idx_cp_part_group ON change_point.cp_part(group_id);

-- =============================================================================
-- CP_DETAILS_CATEGORY — risk detail categories
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_details_category (
    id          SERIAL PRIMARY KEY,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    name        VARCHAR(200)    NOT NULL,
    type        VARCHAR(50),
    is_use      BOOLEAN         NOT NULL DEFAULT TRUE,
    create_by   VARCHAR(20),
    create_date TIMESTAMPTZ     DEFAULT NOW(),
    update_by   VARCHAR(20),
    update_date TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_details_category_group ON change_point.cp_details_category(group_id);

-- =============================================================================
-- CP_DETAILS — risk detail items
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_details (
    id          SERIAL PRIMARY KEY,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    category_id INTEGER         REFERENCES change_point.cp_details_category(id) ON DELETE SET NULL,
    detail      TEXT            NOT NULL,
    risk        VARCHAR(20),
    is_use      BOOLEAN         NOT NULL DEFAULT TRUE,
    create_by   VARCHAR(20),
    create_date TIMESTAMPTZ     DEFAULT NOW(),
    update_by   VARCHAR(20),
    update_date TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_details_group    ON change_point.cp_details(group_id);
CREATE INDEX IF NOT EXISTS idx_cp_details_category ON change_point.cp_details(category_id);

-- =============================================================================
-- CP_CHECK_SHEET — check sheet definitions
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_check_sheet (
    id          SERIAL PRIMARY KEY,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    name        VARCHAR(200)    NOT NULL,
    is_use      BOOLEAN         NOT NULL DEFAULT TRUE,
    create_by   VARCHAR(20),
    create_date TIMESTAMPTZ     DEFAULT NOW(),
    update_by   VARCHAR(20),
    update_date TIMESTAMPTZ     DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_cp_check_sheet_group ON change_point.cp_check_sheet(group_id);

-- =============================================================================
-- CP_CHECK_SHEET_ITEM — check sheet line items
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_check_sheet_item (
    id             SERIAL PRIMARY KEY,
    check_sheet_id INTEGER         NOT NULL REFERENCES change_point.cp_check_sheet(id) ON DELETE CASCADE,
    group_id       INTEGER,
    title          TEXT            NOT NULL,
    item_index     INTEGER         DEFAULT 0,
    text_type      VARCHAR(50),
    status         VARCHAR(20)     DEFAULT 'Y'
);

CREATE INDEX IF NOT EXISTS idx_cp_check_sheet_item_sheet ON change_point.cp_check_sheet_item(check_sheet_id);

-- =============================================================================
-- CP_DASHBOARD — dashboard display configuration
-- =============================================================================
CREATE TABLE IF NOT EXISTS change_point.cp_dashboard (
    id          SERIAL PRIMARY KEY,
    group_id    INTEGER         NOT NULL REFERENCES change_point.cp_group(id) ON DELETE CASCADE,
    priority    INTEGER         DEFAULT 1,
    title       VARCHAR(200),
    time        VARCHAR(50),
    type        VARCHAR(50)
);

CREATE INDEX IF NOT EXISTS idx_cp_dashboard_group ON change_point.cp_dashboard(group_id);
