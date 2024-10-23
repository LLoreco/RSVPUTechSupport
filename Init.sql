-- Create Employee table
CREATE TABLE employees (
    id SERIAL PRIMARY KEY,
    last_name VARCHAR(50) NOT NULL,
    first_name VARCHAR(50) NOT NULL,
    middle_name VARCHAR(50),
    division VARCHAR(150) NOT NULL,
    role VARCHAR(50) NOT NULL,
    email VARCHAR(50) UNIQUE NOT NULL,
    phone INTEGER UNIQUE NOT NULL,
    work_amount INTEGER NOT NULL,
    salary INTEGER NOT NULL,
    status BOOLEAN DEFAULT FALSE,
    password VARCHAR(512) NOT NULL,
    login VARCHAR(512) NOT NULL
);

-- Create Task table
CREATE TABLE tasks (
    id SERIAL PRIMARY KEY,
    work_number INTEGER NOT NULL,
    from_whom_id INTEGER REFERENCES employees(id),
    description TEXT,
    send_time DATE DEFAULT CURRENT_DATE,
    time_limit DATE DEFAULT CURRENT_DATE,
    total_time DATE DEFAULT CURRENT_DATE,
    status VARCHAR(50) NOT NULL DEFAULT 'wait',
    employee_id INTEGER REFERENCES employees(id),
    image VARCHAR(100),
    object_id INTEGER
);

-- Create Object table
CREATE TABLE objects (
    id SERIAL PRIMARY KEY,
    object_name VARCHAR(50) NOT NULL,
    type VARCHAR(50) NOT NULL,
    buy_date DATE DEFAULT CURRENT_DATE,
    break_count INTEGER NOT NULL,
    recovery_date DATE DEFAULT CURRENT_DATE,
    room_number VARCHAR(50) NOT NULL
);

-- Create Breaks table
CREATE TABLE breaks (
    break_types VARCHAR(50) NOT NULL
);

-- Create RecoveryHistory table
CREATE TABLE recoveryhistorys (
    id SERIAL PRIMARY KEY,
    description TEXT,
    employee_id INTEGER REFERENCES employees(id),
    recovery_date DATE DEFAULT CURRENT_DATE,
    total_time DATE DEFAULT CURRENT_DATE,
    object_id INTEGER
);

--Create FAQ table
CREATE TABLE FAQ (
    id SERIAL PRIMARY KEY,
    name VARCHAR(128) NOT NULL,
    description TEXT
);