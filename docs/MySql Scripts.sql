CREATE TABLE `tubumu`.`group` (
  `group_id` char(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '���� Id',
  `parent_id` char(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL COMMENT '�� Id',
  `name` NVARCHAR(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '����',
  `level` INT NOT NULL COMMENT '�㼶',
  `display_order` INT NOT NULL COMMENT '��ʾ˳��',
  `is_contains_user` TINYINT(1) NOT NULL COMMENT '�Ƿ�����û�',
  `is_disabled` TINYINT(1) NOT NULL COMMENT '�Ƿ�ͣ��',
  `is_system` TINYINT(1) NOT NULL COMMENT '�Ƿ���ϵͳ����',
  PRIMARY KEY (`group_id`)),
  INDEX `idx_display_order` (`display_order` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COMMENT = '����';

ALTER TABLE `tubumu`.`group` 
ADD FOREIGN KEY (`parent_id`) REFERENCES `tubumu`.`group` (`group_id`),
ADD INDEX `display_order`(`display_order`) USING BTREE;

